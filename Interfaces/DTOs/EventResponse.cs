using System;

namespace EPCISEvent.Interfaces.DTOs
{
    public class EventResponse
    {
        public string EventId { get; set; }
        public DateTime RecordTime { get; set; }
        public string EventType { get; set; }
    }
}
