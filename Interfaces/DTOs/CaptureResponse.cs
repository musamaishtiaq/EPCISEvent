using System;
using System.Collections.Generic;

namespace EPCISEvent.Interfaces.DTOs
{
    public class CaptureResponse
    {
        public string CaptureId { get; set; }
        public DateTime CaptureTime { get; set; }
        public int EventsProcessed { get; set; }
        public List<string> Warnings { get; set; }
        public List<string> Errors { get; set; }
    }
}
