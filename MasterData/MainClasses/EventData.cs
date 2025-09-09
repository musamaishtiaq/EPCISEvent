using System;

namespace EPCISEvent.MasterData.MainClasses
{
    public class EventData
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string EventLog { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
