using System;

namespace EPCISEvent.Interfaces.DTOs
{
    public class SystemInfo
    {
        public string EPCISVersion { get; set; }
        public string EPCISMinVersion { get; set; }
        public string EPCISMaxVersion { get; set; }
        public string EPCFormat { get; set; }
        public int CaptureLimit { get; set; }
        public string VendorVersion { get; set; }
        public DateTime ServerTime { get; set; }
    }
}
