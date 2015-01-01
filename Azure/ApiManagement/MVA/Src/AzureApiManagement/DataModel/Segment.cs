using System;

namespace Sychev.AzureApiManagement.DataModel
{
	public class Segment
	{
        //public Guid DocumentId { get; set; }

		public long Id { get; set; }

		public int DocumentSegmentId { get; set; }

		public string SourceText { get; set; }

		public string TranslatedText { get; set; }

		public MachineTranslationVariant[] MtVariants { get; set; }
		
		public TranslationMemoryTranslationVariant[] TmVariants { get; set; }

		public Tag[] Tags { get; set; }

		public Revision[] Revisions { get; set; }

		public TermVariant[] Terms { get; set; }
	}
}