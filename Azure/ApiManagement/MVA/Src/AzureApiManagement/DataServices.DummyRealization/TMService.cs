using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;
using Sychev.AzureApiManagement.DataServices;

namespace Sychev.DataServices.DummyRealization
{
    public class TMService : IDocumentResourceService<DocumentTM>
    {
        public void Set(Guid id, List<DocumentTM> data)
        {
            throw new NotImplementedException();
        }

        public List<DocumentTM> Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}