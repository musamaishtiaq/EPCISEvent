using System;
using System.Collections.Generic;

namespace EPCISEvent.Interfaces.DTOs
{
    public class HealthStatus
    {
        public string Status { get; set; }
        public DateTime CheckTime { get; set; }
        public Dictionary<string, string> Components { get; set; }
    }
}
