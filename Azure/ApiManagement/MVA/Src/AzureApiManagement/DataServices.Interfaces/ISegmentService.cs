using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;

namespace Sychev.AzureApiManagement.DataServices
{
    public interface ISegmentService
    {
        List<Segment> Get(Guid id);
    }
}