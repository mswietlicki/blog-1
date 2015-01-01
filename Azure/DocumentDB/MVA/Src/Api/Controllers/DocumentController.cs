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
