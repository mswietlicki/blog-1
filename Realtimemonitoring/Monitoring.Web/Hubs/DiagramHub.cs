using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Sychev.Monitoring.Web.Code.Repository;
using Sychev.Monitoring.Web.Contract.Models.Incoming;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.PushDiagram;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Response;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.Web.Hubs
{
    [HubName("DiagramHub")]
    public class DiagramHub : Hub
    {
        private readonly IPointsRepository _pointsRepository;

        public DiagramHub(IPointsRepository pointsRepository)
        {
            _pointsRepository = pointsRepository;
        }

        public void BroadCastDiagram(BroadCastDiagramModel point)
        {
            var points = point.Points;
            var linesByDiagrams = points.GroupBy(i => i.DiagramId).ToDictionary(i => i.Key, i => i.ToArray());
           
            foreach (var variable in linesByDiagrams)
            {
                var id = variable.Key;
                var sendData = GenerateGenericPushModel(id, points);
                _pointsRepository.PushNewPoints(id, variable.Value);
                Clients.Groups(new[] { id.ToString().ToLower() }).diagramNotify(sendData);
            }
        }

        public void JoinGroup(Guid id)
        {
            Groups.Add(this.Context.ConnectionId, id.ToString().ToLower());
        }


        public void LeftGroup(Guid id)
        {
            Groups.Remove(this.Context.ConnectionId, id.ToString().ToLower());
        }


        public override Task OnConnected()
        {
            return base.OnConnected();
        }


        private static GenericPushModel<UpdateDiagramModel> GenerateGenericPushModel(Guid id, DiagramPointModel[] points)
        {
            var sendData = new GenericPushModel<UpdateDiagramModel>
            {
                Data = new List<UpdateDiagramModel>
                {
                    new UpdateDiagramModel
                    {
                        DiagramId = id,
                        Points = points
                            .Select(i => new DiagramPointModel
                            {
                                X = i.X,
                                Y = i.Y,
                                LineId = i.LineId,
                                DiagramId = i.DiagramId
                            })
                            .ToArray()
                    }
                }
            };
            return sendData;
        }

    }
}