using System;
using System.Collections.Generic;

namespace Sychev.AzureApiManagement.DataServices
{
    public interface IDocumentResourceService <T>
    {
        void Set(Guid id, List<T> data);

        List<T> Get(Guid id);
    }
}
