using System;
using System.Collections.Generic;
using System.Linq;
using Sychev.AzureApiManagement.DataModel;
using Sychev.AzureApiManagement.DataServices;

namespace Sychev.DataServices.DummyRealization
{
    public class SegmentService : ISegmentService
    {
        public List<Segment> Get(Guid id)
        {
            return DocumentService.Data.First(i => i.Id == id).Segments;
        }
    }
}