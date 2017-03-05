using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElertanCheatBase.Exceptions
{
    class InternalPayloadPathNotSetException : Exception
    {
        public InternalPayloadPathNotSetException() : base("If InternalMode is enabled you need to give a path to your payload")
        {
            
        }
    }
}
