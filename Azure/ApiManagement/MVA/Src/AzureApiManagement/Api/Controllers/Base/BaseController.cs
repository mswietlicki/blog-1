using System.Web.Http;

namespace Sychev.AzureApiManagement.Api.Controllers.Base
{
    public abstract class BaseController<T> : ApiController
    {            
        protected readonly T _service;

        protected BaseController(T service)
        {
            _service = service;
        }
    }
}
