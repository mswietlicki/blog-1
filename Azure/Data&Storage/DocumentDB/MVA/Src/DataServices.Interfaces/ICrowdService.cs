using System;
using System.Collections.Generic;
using Sychev.DocumentDB.DataModel;

namespace Sychev.DocumentDB.DataServices
{
    public interface ICrowdService
    {
        List<CrowdVariant> Get(Guid id, int segmentId);
    }
}