using System;
using System.Runtime.Serialization;

namespace Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared
{
    [DataContract]
    public class DiagramPointModel
    {
        [DataMember]
        public double Y { get; set; }

        [DataMember]
        public DateTime X { get; set; }

        [DataMember]
        public Guid LineId { get; set; }

        [DataMember]
        public Guid DiagramId { get; set; }
    }
}
