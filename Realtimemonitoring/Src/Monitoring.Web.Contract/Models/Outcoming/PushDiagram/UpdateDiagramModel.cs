using System;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.Web.Contract.Models.Outcoming.PushDiagram
{
    public class UpdateDiagramModel
    {
        public Guid DiagramId { get; set; }

        public DiagramPointModel[] Points { get; set; }
    }
}