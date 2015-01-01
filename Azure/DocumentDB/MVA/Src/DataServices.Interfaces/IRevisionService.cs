using System;
using System.Collections.Generic;
using Sychev.DocumentDB.DataModel;

namespace Sychev.DocumentDB.DataServices
{
    public interface IRevisionService
    {
        List<Revision> Get(Guid id, int segmentId);
    }
}