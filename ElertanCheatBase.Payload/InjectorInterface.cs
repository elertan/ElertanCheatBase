using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElertanCheatBase.Payload
{
    public class InjectorInterface : MarshalByRefObject
    {
        /// <summary>
        /// Called when the payload has been installed into the target process
        /// </summary>
        /// <param name="clientPid"></param>
        public void IsInstalled(int clientPid)
        {

        }

        /// <summary>
        ///     Called to confirm that the IPC channel is still open / host application has not closed
        /// </summary>
        public void Ping()
        {
        }
    }
}
