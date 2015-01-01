using System;
using System.Collections.Generic;
using Sychev.AzureApiManagement.DataModel;

namespace Sychev.AzureApiManagement.DataServices
{
    public interface IDocumentService
    {
        List<Document> Get();

        Document Get(Guid id);

        void Delete(Guid id);

        Document Add(Document document);
    }
}