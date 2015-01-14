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
    public class WorkflowController : DocumentResourceController<WorkflowService, DocumentWorkflow>
    {
        public WorkflowController()
            : base(new WorkflowService())
        {
        }

        /// <summary>
        /// Set Workflow for document
        /// </summary>
        /// <param name="id">DocumentId</param>
        /// <param name="data">Wrokflow steps collection</param>
        /// <returns></returns>
        [Route("api/Document/{id:guid}/Workflow")]
        [HttpPost, HttpPut]
        [ResponseType(typeof(GenericResponceNoData))]
        public override HttpResponseMessage Set([FromUri] Guid id, [FromBody] List<DocumentWorkflow> data)
        {
            return base.Set(id, data);
        }

        /// <summary>
        /// Get document Workflow
        /// </summary>
        /// <param name="id">DocumentId</param>
        /// <returns></returns>
        [Route("api/Document/{id:guid}/Workflow")]
        [HttpGet]
        [ResponseType(typeof(GenericResponce<DocumentWorkflow>))]
        public override HttpResponseMessage Get([FromUri] Guid id)
        {
            return base.Get(id);
        }
    }
}
