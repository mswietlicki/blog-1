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
    public class TagController : BaseController<ITagService>
    {

        public TagController()
            : base(new TagService())
        {

        }

        /// <summary>
        /// Get segments by DocumentId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="segmentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Document/{id}/Segment/{segmentId}/Tags")]
        [ResponseType(typeof(GenericResponce<Tag>))]
        public HttpResponseMessage Get(Guid id, int segmentId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponce<Tag>(_service.Get(id, segmentId)));
        }
    }
}
