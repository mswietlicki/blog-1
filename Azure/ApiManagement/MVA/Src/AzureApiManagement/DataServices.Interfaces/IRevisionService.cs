using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;

namespace Sychev.AzureApiManagement.DataServices
{
    public interface IRevisionService
    {
        List<Revision> Get(Guid id, int segmentId);
    }
}