﻿using System.Linq;
using System.Runtime.InteropServices;
using ElertanCheatBase.Payload;

namespace ElertanCheatBase.Csgo.Payload.Models
{
    internal class CsLocalPlayer
    {
        public enum CsTeam
        {
            Spectator = 1,
            Terrorist = 2,
            CounterTerrorist = 3
        }

        static CsLocalPlayer()
        {
            var clientModule = Memory.Modules.First(m => m.ModuleName == "client.dll");
            var scanner = new Memory.SignatureScanner
            {
                Address = clientModule.BaseAddress,
                ScanSize = clientModule.ModuleMemorySize
            };

            Memory.AddressResolver.Register(nameof(CsLocalPlayer), () =>
            {
                const string pattern = "88 26 ? ? ? ? ? ? 08 ? ? ? ? ? ? ? ? ? ? ? 88 26";
                var address = scanner.Scan(pattern, 4);
                return Marshal.ReadIntPtr(address);
            });
            Memory.ValueResolver.Register(nameof(Health), () =>
            {
                var address = Memory.AddressResolver.Resolve(nameof(CsLocalPlayer));
                return Memory.ReadInt32(address, 0xFC);
            });
            Memory.ValueResolver.Register(nameof(Team), () =>
            {
                var address = Memory.AddressResolver.Resolve(nameof(CsLocalPlayer));
                return (CsTeam) Memory.ReadInt32(address, 0xF0);
            });
        }

        public static int Health => Memory.ValueResolver.Resolve<int>(nameof(Health));
        public static CsTeam Team => Memory.ValueResolver.Resolve<CsTeam>(nameof(Team));
    }
}