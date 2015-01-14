using System;
using System.Collections.Generic;

namespace Sychev.DocumentDB.DataModel
{
    public class Document
    {
		public Guid Id { get; set; }

		public string Name { get; set; }

		//public byte[] File { get; set; }

		public List<Segment> Segments { get; set; }

		public int SourceLanguageCode { get; set; }

		public int TranslationLanguageCode { get; set; }
    }
}