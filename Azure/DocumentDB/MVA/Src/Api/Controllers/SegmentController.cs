using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SegmentController : BaseController<ISegmentService>
    {

        public SegmentController()
            : base(new SegmentService())
        {

        }

        /// <summary>
        /// Get segments by document id
        /// </summary>
        /// <param name="id">Document Id</param>
        /// <returns>Segments</returns>
        [HttpGet]
        [Route("api/Document/{id}/Segments")]
        [ResponseType(typeof(GenericResponce<Segment>))]
        public HttpResponseMessage Get(Guid id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponce<Segment>(_service.Get(id)));
        }
    }
}
