using System;
using System.Collections.Generic;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.Web.Contract.BL
{
    public interface IDiagramRepository
    {
        List<DiagramModel> GetDiagramsList();
        DiagramModel GetDiagramById(Guid id);
    }
}