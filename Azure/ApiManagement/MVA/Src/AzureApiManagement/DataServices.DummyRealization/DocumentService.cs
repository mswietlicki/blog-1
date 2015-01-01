using System;
using System.Collections.Generic;
using System.Linq;
using Sychev.AzureApiManagement.DataModel;
using Sychev.AzureApiManagement.DataServices;

namespace Sychev.DataServices.DummyRealization
{
    public class DocumentService : IDocumentService
    {
        public static readonly List<Document> Data;

        static DocumentService()
        {
            Data = new List<Document>
            {
               new Document
               {
                  Id = Guid.Parse("88A0D4ED-AEBF-4EF6-ACA6-492873BDC60A"),
                  Name = "Russian Document",
                  SourceLanguageCode = 1049,
                  TranslationLanguageCode = 1033
               },
               new Document
               {
                  Id = Guid.Parse("F94F5051-7750-41B9-920F-6C62BD25DDAD"),
                  Name = "English Document",
                  SourceLanguageCode = 1033,
                  TranslationLanguageCode = 1049
               } 
            };
        }

        public List<Document> Get()
        {
            return Data;
        }

        public Document Get(Guid id)
        {
            return Data.First(i => i.Id == id);
        }

        public void Delete(Guid id)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                if (Data[i].Id == id)
                {
                    Data.RemoveAt(i);
                }
            }
        }

        public Document Add(Document document)
        {
            document.Id = Guid.NewGuid();
            Data.Add(document);
            return document;
        }
    }
}