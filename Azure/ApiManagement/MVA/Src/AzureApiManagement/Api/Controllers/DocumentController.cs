using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Sychev.AzureApiManagement.Api.Controllers.Base;
using Sychev.AzureApiManagement.Api.Models;
using Sychev.AzureApiManagement.DataModel;
using Sychev.AzureApiManagement.DataServices;
using Sychev.DataServices.DummyRealization;

namespace Sychev.AzureApiManagement.Api.Controllers
{
    [RoutePrefix("api/Document")]
    public class DocumentController : BaseController<IDocumentService>
    {
        public DocumentController()
            : base(new DocumentService())
        {

        }

        /// <summary>
        /// Get documents
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(GenericResponce<Document>))]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponce<Document>(_service.Get()));
        }

        /// <summary>
        /// Get document by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(GenericResponce<Document>))]
        public HttpResponseMessage Get([FromUri]Guid id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponce<Document>(_service.Get(id)));
        }

        /// <summary>
        /// Delete document by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(GenericResponceNoData))]
        public HttpResponseMessage Delete([FromUri]Guid id)
        {
            _service.Delete(id);
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponceNoData());
        }

        /// <summary>
        /// Create document
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(GenericResponce<Document>))]
        public HttpResponseMessage Create([FromBody]Document document)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new GenericResponce<Document>(_service.Add(document)));
        }
    }
}
