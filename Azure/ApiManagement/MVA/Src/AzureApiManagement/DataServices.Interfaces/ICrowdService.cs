using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;

namespace Sychev.AzureApiManagement.DataServices
{
    public interface ICrowdService
    {
        List<CrowdVariant> Get(Guid id, int segmentId);
    }
}