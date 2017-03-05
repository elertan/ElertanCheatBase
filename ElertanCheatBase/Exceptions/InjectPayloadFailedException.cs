using System;

namespace ElertanCheatBase
{
        public class InjectPayloadFailedException : Exception
        {
            public InjectPayloadFailedException() : base("Injection failed")
            {
                
            }
        }
}
