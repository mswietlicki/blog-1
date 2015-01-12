using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sychev.Monitoring.Web.Contract.Models.Outcoming.Response
{
    [DataContract]
    public class GenericPushModel<T> where T : class
    {	/// <summary>
        /// Data
        /// </summary> 
        [DataMember]
        public List<T> Data { get; set; }
    }
}