using System;
using System.Collections.Generic;
using Sychev.DocumentDB.DataModel;

namespace Sychev.DocumentDB.DataServices.DocumentDBImpementation
{
    public class WorkflowService: IDocumentResourceService<DocumentWorkflow>
    {
        public void Set(Guid id, List<DocumentWorkflow> data)
        {
            throw new NotImplementedException();
        }

        public List<DocumentWorkflow> Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}