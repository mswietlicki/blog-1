using System;
using System.Collections.Generic;

namespace Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared
{
    public class DiagramModel
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public List<DiagramLineModel> Lines { get; set; }

        public DiagramType DiagramType { get; set; }
    }
}