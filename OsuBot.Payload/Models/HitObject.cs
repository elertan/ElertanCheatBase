using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuBot.Payload.Models
{
    class HitObject
    {
        /// <summary>
        /// Position on the X coordinate
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Position on the Y coordinate
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Time the hit object should be clicked at in milliseconds
        /// </summary>
        public int ClickTime { get; set; }
    }
}
