using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;

namespace Sychev.AzureApiManagement.DataServices
{
    public interface ITagService
    {
        List<Tag> Get(Guid id, int segmentId);
    }
}