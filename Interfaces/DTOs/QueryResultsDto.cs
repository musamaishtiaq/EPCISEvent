using System;
using System.Collections.Generic;

namespace EPCISEvent.Interfaces.DTOs
{
    public class QueryResultsDto
    {
        public string QueryName { get; set; }
        public List<object> EventList { get; set; }
        public int TotalEvents { get; set; }
        public DateTime QueryExecutionTime { get; set; }
    }
}
