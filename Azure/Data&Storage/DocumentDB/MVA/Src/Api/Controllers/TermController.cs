using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Sychev.DocumentDB.Api.Controllers.Base;
using Sychev.DocumentDB.DataModel;
using Sychev.DocumentDB.DataServices.DocumentDBImpementation;

namespace Sychev.DocumentDB.Api.Controllers
{
    //[RoutePrefix("api/Document/{id}/Term")]
    public class TermController : DocumentResourceController<TermService, DocumentTerm>
    {

        public TermController()
            : base(new TermService())
        {

        }

        /// <summary>
        /// Set Term bases for document
        /// </summary>
        /// <param name="id">DocumentId</param>
        /// <param name="data">Term base collection</param>
        /// <returns></returns>
        [Route("api/Document/{id:guid}/Term")]
        [HttpPost,HttpPut]
        [ResponseType(typeof(GenericResponceNoData))]
        public override HttpResponseMessage Set([FromUri] Guid id, [FromBody] List<DocumentTerm> data)
        {
            return base.Set(id, data);
        }

        /// <summary>
        /// Get document Term bases
        /// </summary>
        /// <param name="id">DocumentId</param>
        /// <returns></returns>
        [Route("api/Document/{id:guid}/Term")]
        [HttpGet]
        [ResponseType(typeof(GenericResponce<DocumentTerm>))]
        public override HttpResponseMessage Get([FromUri] Guid id)
        {
            return base.Get(id);
        }
    }
}
