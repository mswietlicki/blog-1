using System;
using System.Collections.Generic;
using Sychev.DocumentDB.DataModel;

namespace Sychev.DocumentDB.DataServices
{
    public interface IDocumentService
    {
        List<Document> Get();

        Document Get(Guid id);

        void Delete(Guid id);

        Document Add(Document document);
    }
}