using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ElertanCheatBase.Exceptions;
using ElertanCheatBase.Payload;

namespace ElertanCheatBase
{
    public class CheatBase
    {
        /// <summary>
        ///     Initializes our cheatbase by processId
        /// </summary>
        /// <param name="targetProcessId"></param>
        public CheatBase(int targetProcessId)
        {
            TargetProcess = Process.GetProcessById(targetProcessId);
        }

        /// <summary>
        ///     Initializes our cheatbase by processName
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="waitForProcess"></param>
        public CheatBase(string processName, bool waitForProcess = false)
        {
            if (waitForProcess) throw new NotImplementedException();
            TargetProcess = Process.GetProcessesByName(processName).First();
        }

        public List<object> InternalPayloadParameters { get; set; } = new List<object>();

        /// <summary>
        ///     Process used to cheat on
        /// </summary>
        public Process TargetProcess { get; }

        /// <summary>
        ///     Determines wether to inject a payload into the target or use external features
        /// </summary>
        public bool InternalMode { get; set; } = true;

        /// <summary>
        ///     The path to the payload library (.dll file)
        /// </summary>
        public string InternalPayloadPath { get; set; }

        public VisualRenderType VisualRenderType { get; set; } = VisualRenderType.None;

        public void Run()
        {
            if (InternalMode)
            {
                if (InternalPayloadPath.Length == 0) throw new InternalPayloadPathNotSetException();
                InternalPayloadParameters.Insert(0, VisualRenderType);
                var parameters = InternalPayloadParameters.ToArray();
                if (!PayloadInjector.InjectPayload(TargetProcess, InternalPayloadPath, parameters))
                    throw new InjectPayloadFailedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}