using EPCISEvent.Fastnt;
using EPCISEvent.Fastnt.CBV;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Action = EPCISEvent.Fastnt.Action;

namespace EPCISEvent.Models
{
    public class ObjectEventViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Event Type")]
        public EventType EventType { get; set; } = EventType.ObjectEvent;

        [Required]
        [Display(Name = "Event Time")]
        public DateTime EventTime { get; set; } = DateTime.UtcNow;

        [Display(Name = "Event Time Zone Offset")]
        public string EventTimeZoneOffset { get; set; } = "+00:00";

        [Display(Name = "Business Step")]
        public BusinessSteps BizStep { get; set; }

        [Display(Name = "Disposition")]
        public Disposition Disposition { get; set; }

        [Display(Name = "Read Point ID")]
        public string ReadPointId { get; set; }

        [Display(Name = "Business Location ID")]
        public string BizLocationId { get; set; }

        [Display(Name = "Destination Sub Location ID")]
        public string DestinationSubLocationId { get; set; }

        [Display(Name = "Destination Location ID")]
        public string DestinationLocationId { get; set; }

        [Display(Name = "Action")]
        public Action Action { get; set; }

        // Lists for complex properties
        public List<BusinessTransaction> BizTransactionList { get; set; } = new List<BusinessTransaction>();
        public List<Source2> SourceList { get; set; } = new List<Source2>();
        public List<Destination2> DestinationList { get; set; } = new List<Destination2>();
        public List<string> EpcList { get; set; } = new List<string>();
        public List<QuantityElement> QuantityList { get; set; } = new List<QuantityElement>();
        public List<Fastnt.InstanceLotMasterDataAttribute> Ilmd { get; set; } = new List<Fastnt.InstanceLotMasterDataAttribute>();

        // Properties for adding new items
        [Display(Name = "New EPC")]
        public string NewEpc { get; set; }

        [Display(Name = "New Business Transaction")]
        public string NewBizTransaction { get; set; }

        [Display(Name = "Transaction Type")]
        public string NewBizTransactionType { get; set; }
    }
}
