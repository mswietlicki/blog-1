using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Sychev.DocumentDB.Api.Controllers.Base;
using Sychev.DocumentDB.DataModel;
using Sychev.DocumentDB.DataServices;
using Sychev.DocumentDB.DataServices.DocumentDBImpementation;

namespace Sychev.DocumentDB.Api.Controllers
{
    public class CrowdController : BaseController<ICrowdService>
    {
        public CrowdController()
            : base(new CrowdService())
        {

        }

        /// <summary>
        /// Get segment crowd translation variant by DocumentId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="segmentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Document/{id}/Segment/{segmentId}/CrowdVariant")]
        [ResponseType(typeof(GenericResponce<CrowdVariant>))]
        public HttpResponseMessage Get([FromUri]Guid id, [FromUri]int segmentId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponce<CrowdVariant>(_service.Get(id, segmentId)));
        }

        /// <summary>
        /// Create segment crowd translation variant by DocumentId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="segmentId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, HttpPut]
        [Route("api/Document/{id}/Segment/{segmentId}/CrowdVariant")]
        [ResponseType(typeof(GenericResponce<CrowdVariant>))]
        public HttpResponseMessage Get([FromUri]Guid id, [FromUri]int segmentId, [FromBody]CrowdVariant data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponce<CrowdVariant>(_service.Get(id, segmentId)));
        }
    }
}
