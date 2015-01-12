using System.Runtime.Serialization;

namespace Sychev.Monitoring.Web.Contract.Models.Outcoming.Response
{
    /// <summary>
    /// 
    /// </summary> 
    [DataContract]
    public class ResponseTemplate
    {
        /// <summary>
        /// Success
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}