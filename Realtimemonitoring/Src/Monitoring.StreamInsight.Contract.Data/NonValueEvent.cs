using System;

namespace Sychev.Monitoring.StreamInsightServer.Models
{
    public class NonValueEvent
    {
        public DateTime Time { get; set; }

        public string Name { get; set; }

        public int TrackEventType { get; set; }
    }
}
