using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ElertanCheatBase.Payload
{
    public static class Memory
    {
        private static uint _pageSize;
        public static uint PageSize
        {
            get
            {
                if (_pageSize != default(uint)) return _pageSize;
                WinApi.GetSystemInfo(out var info);
                _pageSize = info.PageSize;
                return _pageSize;
            }
        }
        public static Process Process { get; private set; }
        public static ProcessModule[] Modules { get; private set; }

        public static void Initialize(Process p)
        {
            Process = p;
            Modules = new ProcessModule[p.Modules.Count];
            for (var i = 0; i < p.Modules.Count; i++)
                Modules[i] = p.Modules[i];
        }

        public static byte ReadByte(IntPtr address, int offset = 0) => Marshal.ReadByte(address, offset);
        public static IntPtr ReadIntPtr(IntPtr address, int offset = 0) => Marshal.ReadIntPtr(address, offset);
        public static byte ReadByte(IntPtr address) => Marshal.ReadByte(address);

        public static byte[] ReadBytes(IntPtr address, int amountOfBytes)
        {
            var buffer = new byte[amountOfBytes];
            for (var i = 0; i < amountOfBytes; i++)
                buffer[i] = ReadByte(IntPtr.Add(address, i));
            return buffer;
        }

        public static byte[] ReadBytes(IntPtr address, int objectSize, params int[] offsets)
        {
            address = offsets.Aggregate(address, ReadIntPtr);
            return ReadBytes(address, objectSize);
        }

        public static float ReadSingle(IntPtr address)
        {
            var bytes = ReadBytes(address, sizeof(float));
            return BitConverter.ToSingle(bytes, 0);
        }

        public static float ReadSingle(IntPtr address, params int[] offsets)
        {
            var bytes = ReadBytes(address, sizeof(float), offsets);
            return BitConverter.ToSingle(bytes, 0);
        }

        public static double ReadDouble(IntPtr address, params int[] offsets)
        {
            var bytes = ReadBytes(address, sizeof(double), offsets);
            return BitConverter.ToDouble(bytes, 0);
        }

        public static int ReadInt32(IntPtr address, int offset = 0) => Marshal.ReadInt32(address, offset);

        public static void WriteBytes(IntPtr address, params byte[] bytes)
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                Marshal.WriteByte(IntPtr.Add(address, i), bytes[i]);
            }
        }

        public static void WriteDouble(IntPtr address, double value)
        {
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(address, bytes);
        }

        /// <summary>
        ///     Provides methods and properties to find memory addresses from the current process.
        /// </summary>
        public class SignatureScanner
        {
            // Should return the base address
            public IntPtr Address { get; set; } = Process.MainModule.BaseAddress;
            public long ScanSize { get; set; } = Process.PagedMemorySize64;

            /// <summary>
            ///     Looks for a pattern in memory and returns the address where the pattern matches
            /// </summary>
            /// <param name="strPattern">
            ///     A pattern string with hexadecimal values for byte values and ?? for wildcards
            ///     Example: 8B 44 24 04 01 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? C2 04 00
            /// </param>
            /// <param name="patternOffset">The offset of the pattern to add or subtract from the address</param>
            /// <returns></returns>
            public IntPtr Scan(string strPattern, int patternOffset = 0)
            {
                var currentAddress = Address;
                var pattern = GetBytePatternByString(strPattern);

                while (currentAddress.ToInt32() < Address.ToInt32() + ScanSize)
                {
                
                    var isReadableMemory = IsReadableMemory(currentAddress, out var regionSize);
                    if (!isReadableMemory)
                    {
                        currentAddress = new IntPtr(currentAddress.ToInt32() + regionSize);
                        continue;
                    }
                    //var buffer = new byte[regionSize];
                    //WinApi.ReadProcessMemory(Process.Handle, currentAddress, buffer, (int)regionSize, out var ptrNumberOfBytesRead);

                    var buffer = ReadBytes(currentAddress, (int)regionSize);

                    for (var i = 0; i < regionSize; i++)
                    {
                        var foundBytes = true;
                        for (var x = 0; x < pattern.Length; x++)
                        {
                            if (!pattern[x].HasValue) continue;

                            if (pattern[x].Value != buffer[i + x])
                            {
                                foundBytes = false;
                                break;
                            }
                        }

                        if (foundBytes)
                        {
                            return new IntPtr(currentAddress.ToInt32() + i + patternOffset);
                        }
                    }

                    currentAddress = new IntPtr(currentAddress.ToInt32() + regionSize);
                }
                return IntPtr.Zero;
            }

            private static bool IsReadableMemory(IntPtr address, out uint regionSize)
            {
                const int MEM_COMMIT = 0x00001000;
                const int MEM_PRIVATE = 0x20000;
                const int MEM_IMAGE = 0x1000000;
                const int PAGE_NOACCESS = 0x01;
                const int PAGE_READONLY = 0x02;
                const int PAGE_READWRITE = 0x04;
                const int PAGE_GUARD = 0x100;

                var mbi = new MEMORY_BASIC_INFORMATION();

                WinApi.VirtualQuery(address, ref mbi, Marshal.SizeOf<MEMORY_BASIC_INFORMATION>());

                regionSize = (uint)mbi.RegionSize.ToInt32();

                return mbi.State == MEM_COMMIT
                    && !Convert.ToBoolean(mbi.Protect & PAGE_GUARD)
                    && mbi.Protect > PAGE_NOACCESS
                    && mbi.Type == MEM_PRIVATE;
            }

            private static byte?[] GetBytePatternByString(string strPattern)
            {
                return strPattern.Split(' ').Select(str =>
                {
                    var nullableConverter = new NullableConverter(typeof(byte?));
                    try
                    {
                        var value = Convert.ToByte(str, 16);
                        nullableConverter.ConvertFrom(value);
                        return (byte?)nullableConverter.ConvertFrom(value);
                    }
                    catch
                    {
                        return default(byte?);
                    }
                }).ToArray();
            }
        }

        public class Dumper
        {
            public IntPtr Address { get; set; } = IntPtr.Zero;
            public int Size { get; set; } = 4096;

            public byte[] Dump()
            {
                // Invalid arguments
                if (Size == 0) throw new Exception("Dumpsize can't be 0");
                if (Address == IntPtr.Zero) throw new Exception("Address cant be 0x0");

                return ReadBytes(Address, Size);
            }
        }

        public static class AddressResolver
        {
            private static readonly Dictionary<string, Func<IntPtr>> ResolvingFunctions =
                new Dictionary<string, Func<IntPtr>>();

            private static readonly Dictionary<string, IntPtr> ResolvedAddresses = new Dictionary<string, IntPtr>();

            public static void Register(string key, Func<IntPtr> resolvingFunc)
            {
                if (ResolvingFunctions.ContainsKey(key))
                    throw new Exception($"Can't register more than 1 resolving method per key ({key})");

                ResolvingFunctions.Add(key, resolvingFunc);
            }

            public static IntPtr Resolve(string key, bool resolveOnce = true)
            {
                if (resolveOnce && ResolvedAddresses.ContainsKey(key)) return ResolvedAddresses[key];

                if (!ResolvingFunctions.ContainsKey(key))
                    throw new Exception($"There is no existing registered resolve for the key: {key}");
                var pointer = ResolvingFunctions[key].Invoke();

                if (resolveOnce) ResolvedAddresses.Add(key, pointer);

                return pointer;
            }
        }

        public static class ValueResolver
        {
            private static readonly Dictionary<string, Func<object>> ResolvingFunctions =
                new Dictionary<string, Func<object>>();

            private static readonly Dictionary<string, object> ResolvedObjects = new Dictionary<string, object>();

            public static void Register(string key, Func<object> resolvingFunc)
            {
                if (ResolvingFunctions.ContainsKey(key))
                    throw new Exception($"Can't register more than 1 resolving method per key ({key})");

                ResolvingFunctions.Add(key, resolvingFunc);
            }

            public static T Resolve<T>(string key, bool resolveOnce = false)
            {
                if (resolveOnce && ResolvedObjects.ContainsKey(key)) return (T) ResolvedObjects[key];

                if (!ResolvingFunctions.ContainsKey(key))
                    throw new Exception($"There is no existing registered resolve for the key: {key}");
                var value = ResolvingFunctions[key].Invoke();

                if (resolveOnce) ResolvedObjects.Add(key, value);

                return (T) value;
            }
        }
    }
}