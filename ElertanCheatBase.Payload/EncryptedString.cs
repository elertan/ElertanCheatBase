﻿using System.Linq;

namespace ElertanCheatBase.Payload
{
    public class EncryptedString
    {
        public EncryptedString()
        {
        }

        public EncryptedString(string originalString)
        {
            EncryptedData = EncryptString(originalString);
        }

        public string EncryptedData { get; set; }

        public string Value
        {
            get { return DecryptString(EncryptedData); }
            set { EncryptedData = EncryptString(value); }
        }

        private string EncryptString(string str)
        {
            if (str.Length == 0) return str;
            str = new string(str.Reverse().ToArray());
            if (str.Length > 2)
                str = str.Substring(1, str.Length - 1) + str.First();
            return str;
        }

        private string DecryptString(string str)
        {
            if (str.Length == 0) return str;
            if (str.Length > 2)
                str = str.Last() + str.Substring(0, str.Length - 1);
            str = new string(str.Reverse().ToArray());
            return str;
        }
    }
}