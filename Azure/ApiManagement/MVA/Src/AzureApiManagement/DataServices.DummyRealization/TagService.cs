using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;
using Sychev.AzureApiManagement.DataServices;

namespace Sychev.DataServices.DummyRealization
{
    public class TagService : ITagService
    {
        public List<Tag> Get(Guid id, int segmentId)
        {
            throw new NotImplementedException();
        }
    }
}