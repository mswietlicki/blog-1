using System;

namespace Sychev.Monitoring.StreamInsightServer.Models
{

    [Serializable]
    public class SerializableException
    {
        public DateTime TimeStamp { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public SerializableException()
        {
            this.TimeStamp = DateTime.Now;
        }

        public SerializableException(string message)
            : this()
        {
            this.Message = message;
        }

        public SerializableException(Exception ex)
            : this(ex.Message)
        {
            this.StackTrace = ex.StackTrace;
        }

        public override string ToString()
        {
            return this.Message + this.StackTrace;
        }
    }
}