using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using EasyHook;
using ElertanCheatBase.Payload;

namespace ElertanCheatBase
{
    class PayloadInjector
    {

        /// <summary>
        /// Injects our payload into the process
        /// </summary>
        /// <param name="process">the process</param>
        /// <param name="injectionLibrary">the payload lib</param>
        /// <param name="passParams">Parameters to be passed</param>
        /// <returns>Did work correctly?</returns>
        public static bool InjectPayload(Process process, string injectionLibrary, params object[] passParams)
        {
            string channelName = null;
            // Create IPC server
            RemoteHooking.IpcCreateServer<InjectorInterface>(ref channelName, WellKnownObjectMode.Singleton);
            // Get our payload library path
            try
            {
                //Config.Register("ElertanCheatBase",
                //    Payload.Core.AssemblyPath,
                //    injectionLibrary);
                // Inject payload
                RemoteHooking.Inject(
                    process.Id, // ID of process to inject into
                    injectionLibrary, // 32-bit library to inject (if target is 32-bit)
                    injectionLibrary, // 64-bit library to inject (if target is 64-bit)
                    channelName 
                    // the parameters to pass into injected library
                );
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
