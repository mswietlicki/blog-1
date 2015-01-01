using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;
using Sychev.AzureApiManagement.DataServices;

namespace Sychev.DataServices.DummyRealization
{
    public class TermService : IDocumentResourceService<DocumentTerm>
    {
        public void Set(Guid id, List<DocumentTerm> data)
        {
            throw new NotImplementedException();
        }

        public List<DocumentTerm> Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}