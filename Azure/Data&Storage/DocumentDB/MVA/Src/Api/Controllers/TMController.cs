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
    public class TMController : DocumentResourceController<TMService, DocumentTM>
    {

        public TMController()
            : base(new TMService())
        {

        }

        /// <summary>
        /// Set TM for document
        /// </summary>
        /// <param name="id">DocumentId</param>
        /// <param name="data">TM collection</param>
        /// <returns></returns>
        [Route("api/Document/{id:guid}/TM")]
        [HttpPost, HttpPut]
        [ResponseType(typeof(GenericResponceNoData))]
        public override HttpResponseMessage Set([FromUri] Guid id, [FromBody] List<DocumentTM> data)
        {
            return base.Set(id, data);
        }

        /// <summary>
        /// Get document TM
        /// </summary>
        /// <param name="id">DocumentId</param>
        /// <returns></returns>
        [Route("api/Document/{id:guid}/TM")]
        [HttpGet]
        [ResponseType(typeof(GenericResponce<DocumentTM>))]
        public override HttpResponseMessage Get([FromUri] Guid id)
        {
            return base.Get(id);
        }
    }
}