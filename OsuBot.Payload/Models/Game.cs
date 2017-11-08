using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ElertanCheatBase.Payload;

namespace OsuBot.Payload.Models
{
    class Game
    {
        private const string GameSongTimeSignatureScan1 = "SongTimeSig1";

        static Game()
        {
            var signatureScanner = new Memory.SignatureScanner();

            Memory.AddressResolver.Register(GameSongTimeSignatureScan1, () =>
            {
                var address = signatureScanner.Scan("DD 45 EC DD 1D", 5);
                return address;
            });

            Memory.AddressResolver.Register(nameof(SongTime), () =>
            {
                var address = Memory.AddressResolver.Resolve(GameSongTimeSignatureScan1);
                return Memory.ReadIntPtr(address);
            });

            Memory.ValueResolver.Register(nameof(SongTime), () =>
            {
                var address = Memory.AddressResolver.Resolve(nameof(SongTime));
                return Memory.ReadDouble(address);
            });
        }

        public static double SongTime
        {
            get => Memory.ValueResolver.Resolve<double>(nameof(SongTime));
            set
            {
                var address = Memory.AddressResolver.Resolve(nameof(SongTime));
                Memory.WriteDouble(address, value);
            }
        }
    }
}
