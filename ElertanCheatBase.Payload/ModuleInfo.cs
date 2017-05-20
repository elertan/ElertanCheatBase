using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElertanCheatBase.Payload
{
    public class ModuleInfo
    {
        public string Name { get; set; }
        public int MemorySize { get; set; }
        public IntPtr Address { get; set; }
    }
}
