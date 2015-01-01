using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;
using Sychev.AzureApiManagement.DataServices;

namespace Sychev.DataServices.DummyRealization
{
    public class RevisionService : IRevisionService
    {
        public List<Revision> Get(Guid id, int segmentId)
        {
            throw new NotImplementedException();
        }
    }
}