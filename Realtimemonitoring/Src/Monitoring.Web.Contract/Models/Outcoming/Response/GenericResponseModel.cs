using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sychev.Monitoring.Web.Contract.Models.Outcoming.Response
{
    public class GenericResponseModel<T> : ResponseTemplate where T : class
    {	/// <summary>
		/// Data
		/// </summary> 
		[DataMember]
		public List<T> Data { get; set; }
    }
}