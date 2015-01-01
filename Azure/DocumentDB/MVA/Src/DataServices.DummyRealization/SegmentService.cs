using System;
using System.Collections.Generic;
using System.Linq;
using Sychev.DocumentDB.DataModel;

namespace Sychev.DocumentDB.DataServices.DocumentDBImpementation
{
    public class SegmentService : ISegmentService
    {
        public List<Segment> Get(Guid id)
        {
            return DocumentService.Data.First(i => i.Id == id).Segments;
        }
    }
}