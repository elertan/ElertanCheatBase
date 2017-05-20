using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace ElertanCheatBase.Payload
{
    public static class Memory
    {
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

        /// <summary>
        ///     Provides methods and properties to find memory addresses from the current process.
        /// </summary>
        public class SignatureScanner
        {
            //private byte[] _dump;
            public IntPtr Address { get; set; } = IntPtr.Zero;
            public int ScanSize { get; set; } = 4096;

            /// <summary>
            ///     Looks for a pattern in memory and returns the address where the pattern matches
            /// </summary>
            /// <param name="strPattern">
            ///     A pattern string with hexadecimal values for byte values and ?? for wildcards
            ///     Example: 8B 44 24 04 01 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? C2 04 00
            /// </param>
            /// <returns></returns>
            public IntPtr Scan(string strPattern)
                // pattern example 8B 44 24 04 01 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? C2 04 00
            {
                //DumpMemory();

                // Memory Dumping failed
                var pattern = strPattern.Split(' ').Select(str =>
                {
                    var nullableConverter = new NullableConverter(typeof(int?));
                    try
                    {
                        var value = Convert.ToInt32(str, 16);
                        nullableConverter.ConvertFrom(value);
                        return (int?) nullableConverter.ConvertFrom(value);
                    }
                    catch
                    {
                        return default(int?);
                    }
                }).ToArray();

                for (var i = 0; i < ScanSize; i++)
                {
                    var patternMatch = true;
                    for (var x = 0; x < pattern.Length; x++)
                    {
                        if (!pattern[x].HasValue) continue;

                        if (pattern[x].Value != ReadByte(Address + i + x))
                        {
                            patternMatch = false;
                            break;
                        }
                    }
                    if (patternMatch)
                    {
                        var address = IntPtr.Add(Address, i);
                        return address;
                    }
                }
                throw new Exception("Address not found for given pattern");
            }

            //private void DumpMemory()
            //{
            //    // Invalid arguments
            //    if (Size == 0) throw new Exception("Dumpsize can't be 0");
            //    if (Address == IntPtr.Zero) throw new Exception("Address cant be 0x0");

            //    _dump = new byte[Size];

            //    _dump = ReadBytes(Address, Size);
            //}
        }
    }
}