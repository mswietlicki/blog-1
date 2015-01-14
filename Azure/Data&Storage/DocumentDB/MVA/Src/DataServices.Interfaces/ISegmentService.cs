using System;
using System.Collections.Generic;
using Sychev.DocumentDB.DataModel;

namespace Sychev.DocumentDB.DataServices
{
    public interface ISegmentService
    {
        List<Segment> Get(Guid id);
    }
}