using System;
using System.Collections.Generic;

namespace EPCISEvent.Interfaces.DTOs
{
    public class QueryParameters
    {
        public List<string> EventType { get; set; }
        public List<string> EQ_bizStep { get; set; }
        public List<string> EQ_action { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public List<string> EPCs { get; set; }
        // Add more query parameters as needed
    }
}
