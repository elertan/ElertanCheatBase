using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElertanCheatBase.Payload.Interfaces
{
    interface IHook
    {
        void Install(HookBase hookBase);
        void Uninstall();
    }
}
