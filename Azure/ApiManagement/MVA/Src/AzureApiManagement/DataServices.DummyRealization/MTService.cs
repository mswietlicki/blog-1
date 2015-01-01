using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;
using Sychev.AzureApiManagement.DataServices;

namespace Sychev.DataServices.DummyRealization
{
    public class MTService :IDocumentResourceService <DocumentMT>
    {
        public void Set(Guid id, List<DocumentMT> data)
        {
            throw new NotImplementedException();
        }

        public List<DocumentMT> Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}