﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElertanCheatBase.Payload;

namespace ElertanCheatBase
{
    public class CheatBase
    {
        /// <summary>
        /// Process used to cheat on
        /// </summary>
        public Process TargetProcess { get; private set; }
        /// <summary>
        /// Determines wether to inject a payload into the target or use external features
        /// </summary>
        public bool InternalMode { get; set; } = true;
        /// <summary>
        /// The path to the payload library (.dll file)
        /// </summary>
        public string InternalPayloadPath { get; set; }

        public VisualRenderType VisualRenderType { get; set; } = VisualRenderType.None;
        /// <summary>
        /// Initializes our cheatbase by processId
        /// </summary>
        /// <param name="targetProcessId"></param>
        public CheatBase(int targetProcessId)
        {
            TargetProcess = Process.GetProcessById(targetProcessId);
        }

        /// <summary>
        /// Initializes our cheatbase by processName
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="waitForProcess"></param>
        public CheatBase(string processName, bool waitForProcess = false)
        {
            if (waitForProcess) throw new NotImplementedException();
            TargetProcess = Process.GetProcessesByName(processName).First();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}