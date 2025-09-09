namespace EPCISEvent.Interfaces.DTOs
{
    public class SubscriptionRequest
    {
        public string Dest { get; set; }
        public string SignatureToken { get; set; }
        public bool ReportIfEmpty { get; set; }
        public SubscriptionSchedule Schedule { get; set; }
    }
}
