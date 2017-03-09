using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ElertanCheatBase.Payload
{
    public static class Memory
    {
        public static ProcessModule[] Modules { get; private set; }

        public static void Initialize(Process p)
        {
            Modules = new ProcessModule[p.Modules.Count];
            for (var i = 0; i < p.Modules.Count; i++)
            {
                Modules[i] = p.Modules[i];
            }
        }

        public static byte ReadByte(IntPtr address, int offset = 0) => Marshal.ReadByte(address, offset);
        public static byte[] ReadBytes(IntPtr address, int amountOfBytes)
        {
            var buffer = new byte[amountOfBytes];
            for (var i = 0; i < amountOfBytes; i++)
                buffer[i] = Marshal.ReadByte(IntPtr.Add(address, i));
            return buffer;
        }

        public static int ReadInt32(IntPtr address, int offset = 0) => Marshal.ReadInt32(address, offset);

        /// <summary>
        ///     Provides methods and properties to find memory addresses from the current process.
        /// </summary>
        public class SignatureScanner
        {
            public IntPtr Address { get; set; } = IntPtr.Zero;
            public int Size { get; set; } = 4096;

            /// <summary>
            ///     Looks for a pattern in dumped memory and returns the address where the pattern matches
            /// </summary>
            /// <param name="strPattern">
            ///     A pattern string with hexadecimal values for byte values and ?? for wildcards
            ///     Example: 8B 44 24 04 01 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? C2 04 00
            /// </param>
            /// <param name="patternOffset">The offset of the pattern to add or subtract from the address</param>
            /// <returns></returns>
            public IntPtr Scan(string strPattern, int patternOffset = 0)
            {
                
                var pattern = strPattern.Split(' ').Select(str =>
                {
                    var nullableConverter = new NullableConverter(typeof(byte?));
                    try
                    {
                        var value = Convert.ToByte(str, 16);
                        nullableConverter.ConvertFrom(value);
                        return (byte?) nullableConverter.ConvertFrom(value);
                    }
                    catch
                    {
                        return default(byte?);
                    }
                }).ToArray();

                for (var i = 0; i < Size; i++)
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
                    if (patternMatch) return IntPtr.Add(Address + patternOffset, i);
                }
                throw new Exception("Address not found for given pattern");
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
    }
}