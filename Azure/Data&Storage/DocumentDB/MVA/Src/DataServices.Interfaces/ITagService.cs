using System;
using System.Collections.Generic;
using Sychev.DocumentDB.DataModel;

namespace Sychev.DocumentDB.DataServices
{
    public interface ITagService
    {
        List<Tag> Get(Guid id, int segmentId);
    }
}