using System;

namespace EPCISEvent.Interfaces.DTOs
{
    public class SubscriptionDto
    {
        public string Id { get; set; }
        public string QueryName { get; set; }
        public string Destination { get; set; }
        public SubscriptionSchedule Schedule { get; set; }
        public bool ReportIfEmpty { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
