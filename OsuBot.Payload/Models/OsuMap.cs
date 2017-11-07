using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuBot.Payload.Models
{
    class OsuMap
    {
        public string Name { get; set; }
        public string OsuFilePath { get; set; }
        public List<HitObject> HitObjects { get; set; }
        public List<TimingPoint> TimingPoints { get; set; }

        public async Task LoadMap(string osuFilePath)
        {
            OsuFilePath = osuFilePath;

            //
        }
    }
}
