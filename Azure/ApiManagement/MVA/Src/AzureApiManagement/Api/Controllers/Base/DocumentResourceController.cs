using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sychev.AzureApiManagement.Api.Models;
using Sychev.AzureApiManagement.DataServices;

namespace Sychev.AzureApiManagement.Api.Controllers.Base
{
    public abstract class DocumentResourceController<T, TM> : BaseController<T> 
        where T : IDocumentResourceService<TM>
    {
        protected DocumentResourceController(T service)
            : base(service)
        {
        }

        public virtual HttpResponseMessage Set([FromUri] Guid id, [FromBody] List<TM> data)
        {
            _service.Set(id, data);
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponceNoData());
        }

        public virtual HttpResponseMessage Get([FromUri] Guid id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponce<TM>(_service.Get(id)));
        }
    }
}
