using System;
using System.Collections.Generic;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.Web.Code.Repository
{
    public interface IPointsRepository
    {
        List<DiagramPointModel> GetPoinsByDiagram(Guid id);
        void PushNewPoints(Guid id, IEnumerable<DiagramPointModel> newPoints);
    }
}