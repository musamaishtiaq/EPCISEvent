using EPCISEvent.Fastnt;
using EPCISEvent.Fastnt.CBV;
using System.Collections.Generic;

namespace EPCISEvent.Helpers
{
    public static class BusinessEventDispositionMappings
    {
        public static readonly Dictionary<BusinessEventType, List<Disposition>> Mapping
            = new Dictionary<BusinessEventType, List<Disposition>>
            {
            {
                BusinessEventType.Commissioning,
                new List<Disposition>
                {
                    Disposition.Active
                }
            },

            {
                BusinessEventType.Packing,
                new List<Disposition>
                {
                    Disposition.Active
                }
            },

            {
                BusinessEventType.Shipping,
                new List<Disposition>
                {
                    Disposition.InTransit
                }
            },

            {
                BusinessEventType.Receiving,
                new List<Disposition>
                {
                    Disposition.InProgress,
                    Disposition.Active    // once accepted
                }
            },

            {
                BusinessEventType.Unpacking,
                new List<Disposition>
                {
                    Disposition.InProgress
                }
            },

            {
                BusinessEventType.VoidShipping,
                new List<Disposition>
                {
                    Disposition.InProgress
                }
            },

            {
                BusinessEventType.Inspection,
                new List<Disposition>
                {
                    Disposition.InProgress,
                    Disposition.NonSellableOther,
                    Disposition.Damaged,
                    Disposition.Active // after release
                }
            },

            {
                BusinessEventType.Decommissioning,
                new List<Disposition>
                {
                    Disposition.Destroyed,
                    Disposition.Expired,
                    Disposition.Inactive,
                    Disposition.Disposed
                }
            },

            {
                BusinessEventType.Dispensing,
                new List<Disposition>
                {
                    Disposition.Dispensed
                }
            }
            };
    }
}
