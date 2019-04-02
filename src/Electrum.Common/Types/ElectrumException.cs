using System;

namespace Electrum.Common.Types
{
    public class ElectrumException : Exception
    {
        public string Code { get; set; }
        public ElectrumException()
        {
        }

        public ElectrumException(string code)
        {
            Code = code;
        }

        public ElectrumException(string code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        public ElectrumException(Exception innerException, string code, string message, params object[] args) : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}