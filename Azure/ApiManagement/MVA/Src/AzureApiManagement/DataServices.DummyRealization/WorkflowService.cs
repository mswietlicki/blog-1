using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;
using Sychev.AzureApiManagement.DataServices;

namespace Sychev.DataServices.DummyRealization
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