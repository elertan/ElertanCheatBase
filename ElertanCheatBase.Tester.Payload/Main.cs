using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElertanCheatBase.Tester.Payload
{
    public class Main : ElertanCheatBase.Payload.Base
    {
        public static string AssemblyPath => Assembly.GetExecutingAssembly().Location;

        public Main() : base()
        {
            
        }
    }
}
