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
    public class MTController : DocumentResourceController<MTService, DocumentMT>
    {
        public MTController()
            : base(new MTService())
        {

        }

        /// <summary>
        /// Set MT for document
        /// </summary>
        /// <param name="id">DocumentId</param>
        /// <param name="data">MT collection</param>
        /// <returns></returns>
        [Route("api/Document/{id:guid}/MT")]
        [HttpPost, HttpPut]
        [ResponseType(typeof(GenericResponceNoData))]
        public override HttpResponseMessage Set([FromUri] Guid id, [FromBody] List<DocumentMT> data)
        {
            return base.Set(id, data);
        }

        /// <summary>
        /// Get document MT
        /// </summary>
        /// <param name="id">DocumentId</param>
        /// <returns></returns>
        [Route("api/Document/{id:guid}/MT")]
        [HttpGet]
        [ResponseType(typeof(GenericResponce<DocumentMT>))]
        public override HttpResponseMessage Get([FromUri] Guid id)
        {
            return base.Get(id);
        }
    }
}
