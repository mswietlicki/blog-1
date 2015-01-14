using System;
using System.Collections.Generic;
using Sychev.DocumentDB.DataModel;

namespace Sychev.DocumentDB.DataServices.DocumentDBImpementation
{
    public class MTService :IDocumentResourceService <DocumentMT>
    {
        public void Set(Guid id, List<DocumentMT> data)
        {
            throw new NotImplementedException();
        }

        public List<DocumentMT> Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}