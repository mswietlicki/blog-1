using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sychev.Monitoring.Web.Code.Repository;
using Sychev.Monitoring.Web.Contract.BL;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.PushDiagram;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Response;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.Web.Api
{
    public sealed class DiagramController : ApiController
    {
        private readonly IDiagramRepository _diagramRepository;
        private readonly IPointsRepository _pointsRepository;
        public DiagramController()
            : this(new DiagramRepository(), new PointsRepository())
        {

        }

        public DiagramController(IDiagramRepository diagramRepository, IPointsRepository pointsRepository)
        {
            _diagramRepository = diagramRepository;
            _pointsRepository = pointsRepository;
        }

        [HttpGet]
        public HttpResponseMessage GetDiagrams()
        {
            var diagrams = _diagramRepository.GetDiagramsList();
            var toRet = new GenericResponseModel<DiagramModel>
            {
                Message = "Ok",
                Success = true,
                Data = diagrams
            };

            return Request.CreateResponse(HttpStatusCode.OK, toRet);
        }

        [HttpGet]
        public HttpResponseMessage GetDiagram(Guid id)
        {
            var diagram = _diagramRepository.GetDiagramsList().FirstOrDefault(i => i.Id == id);
            if (diagram == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("id= {0} not find", id));

            var toRet = new GenericResponseModel<DiagramModel>
            {
                Message = "Ok",
                Success = true,
                Data = new List<DiagramModel>
                {
                    diagram
                }
            };

            return Request.CreateResponse(HttpStatusCode.OK, toRet);
        }

        [HttpGet]
        public HttpResponseMessage GetDiagramData(Guid id)
        {
            var diagram = _diagramRepository.GetDiagramById(id);
            var points = _pointsRepository.GetPoinsByDiagram(id);

            var toRet = GenerateGenericPushModel(id, points);
            return Request.CreateResponse(HttpStatusCode.OK, toRet);
        }

        private static GenericResponseModel<UpdateDiagramModel> GenerateGenericPushModel(Guid id, IEnumerable<DiagramPointModel> points)
        {
            var sendData = new GenericResponseModel<UpdateDiagramModel>
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
                },
                Message = "Ok",
                Success = true,
            };
            return sendData;
        }
    }
}
