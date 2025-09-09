//using Newtonsoft.Json;
//using EPCISEvent.Fastnt.CBV;
//using EPCISEvent.Fastnt.SBDH;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EPCISEvent.Fastnt
//{
//    public class TemplateMixin
//    {
//        protected bool _renderXmlDeclaration;
//        protected Dictionary<string, object> _context;

//        public TemplateMixin(bool renderXmlDeclaration = false)
//        {
//            _renderXmlDeclaration = renderXmlDeclaration;
//            _context = new Dictionary<string, object>
//            {
//                { "event", this },
//                { "render_xml_declaration", _renderXmlDeclaration }
//            };
//        }

//        public virtual List<string> Namespaces => new List<string>();

//        public virtual string Render()
//        {
//            // This would be implemented with actual template rendering logic
//            // For now we'll just return JSON representation
//            return JsonFormatMixin.RenderJson(this);
//        }
//    }

//    public class RenderObjectEvent : TemplateMixin
//    {
//        private readonly ObjectEvent _oevent;
//        public ObjectEventEncoder _encoder { get; } = new ObjectEventEncoder();

//        public RenderObjectEvent(
//            string eventTime = null,
//            string eventTimezoneOffset = "+00:00",
//            string recordTime = null,
//            string action = "ADD",
//            List<string> epcList = null,
//            string bizStep = null,
//            string disposition = null,
//            string readPoint = null,
//            string bizLocation = null,
//            string eventId = null,
//            ErrorDeclaration errorDeclaration = null,
//            List<Source> sourceList = null,
//            List<Destination> destinationList = null,
//            List<BusinessTransaction> businessTransactionList = null,
//            List<InstanceLotMasterDataAttribute> ilmd = null,
//            List<QuantityElement> quantityList = null,
//            bool renderXmlDeclaration = false)
//        {
//            _oevent = new ObjectEvent(
//                eventTime ?? DateTime.UtcNow.ToString("o"),
//                eventTimezoneOffset,
//                recordTime,
//                action,
//                epcList,
//                bizStep,
//                disposition,
//                readPoint,
//                bizLocation,
//                eventId,
//                errorDeclaration,
//                sourceList,
//                destinationList,
//                businessTransactionList,
//                ilmd,
//                quantityList);
//            _renderXmlDeclaration = renderXmlDeclaration;
//        }

//        public override List<string> Namespaces
//        {
//            get
//            {
//                var namespaces = new List<string>();
//                if (_oevent.Ilmd != null && _oevent.Ilmd.Count > 0)
//                {
//                    namespaces.Add("xmlns:cbvmd=\"urn:epcglobal:cbv:mda\"");
//                }
//                return namespaces;
//            }
//        }
//    }

//    public class RenderAggregationEvent : TemplateMixin
//    {
//        private readonly AggregationEvent _aevent;
//        public AggregationEventEncoder _encoder { get; } = new AggregationEventEncoder();

//        public RenderAggregationEvent(
//            string eventTime = null,
//            string eventTimezoneOffset = "+00:00",
//            string recordTime = null,
//            string action = "ADD",
//            string parentId = null,
//            List<string> childEpcs = null,
//            List<QuantityElement> childQuantityList = null,
//            string bizStep = null,
//            string disposition = null,
//            string readPoint = null,
//            string bizLocation = null,
//            string eventId = null,
//            ErrorDeclaration errorDeclaration = null,
//            List<Source> sourceList = null,
//            List<Destination> destinationList = null,
//            List<BusinessTransaction> businessTransactionList = null,
//            bool renderXmlDeclaration = false)
//        {
//            _aevent = new AggregationEvent(
//                eventTime ?? DateTime.UtcNow.ToString("o"),
//                eventTimezoneOffset,
//                recordTime,
//                action,
//                parentId,
//                childEpcs,
//                childQuantityList,
//                bizStep,
//                disposition,
//                readPoint,
//                bizLocation,
//                eventId,
//                errorDeclaration,
//                sourceList,
//                destinationList,
//                businessTransactionList);
//            _renderXmlDeclaration = renderXmlDeclaration;
//        }
//    }

//    public class RenderTransactionEvent : TemplateMixin
//    {
//        private readonly TransactionEvent _tevent;
//        public TransactionEventEncoder _encoder { get; } = new TransactionEventEncoder();

//        public RenderTransactionEvent(
//            string eventTime = null,
//            string eventTimezoneOffset = "+00:00",
//            string recordTime = null,
//            string action = "ADD",
//            string parentId = null,
//            List<string> epcList = null,
//            string bizStep = null,
//            string disposition = null,
//            string readPoint = null,
//            string bizLocation = null,
//            string eventId = null,
//            ErrorDeclaration errorDeclaration = null,
//            List<Source> sourceList = null,
//            List<Destination> destinationList = null,
//            List<BusinessTransaction> businessTransactionList = null,
//            List<QuantityElement> quantityList = null,
//            bool renderXmlDeclaration = false)
//        {
//            _tevent = new TransactionEvent(
//                eventTime ?? DateTime.UtcNow.ToString("o"),
//                eventTimezoneOffset,
//                recordTime,
//                action,
//                parentId,
//                epcList,
//                bizStep,
//                disposition,
//                readPoint,
//                bizLocation,
//                eventId,
//                errorDeclaration,
//                sourceList,
//                destinationList,
//                businessTransactionList,
//                quantityList);
//            _renderXmlDeclaration = renderXmlDeclaration;
//        }
//    }

//    public class RenderTransformationEvent : TemplateMixin
//    {
//        private readonly TransformationEvent _tevent;
//        public TransformationEventEncoder _encoder { get; } = new TransformationEventEncoder();

//        public RenderTransformationEvent(
//            string eventTime = null,
//            string eventTimezoneOffset = "+00:00",
//            string recordTime = null,
//            string eventId = null,
//            List<string> inputEpcList = null,
//            List<QuantityElement> inputQuantityList = null,
//            List<string> outputEpcList = null,
//            List<QuantityElement> outputQuantityList = null,
//            string transformationId = null,
//            string bizStep = null,
//            string disposition = null,
//            string readPoint = null,
//            string bizLocation = null,
//            List<BusinessTransaction> businessTransactionList = null,
//            List<Source> sourceList = null,
//            List<Destination> destinationList = null,
//            List<InstanceLotMasterDataAttribute> ilmd = null,
//            ErrorDeclaration errorDeclaration = null,
//            bool renderXmlDeclaration = false)
//        {
//            _tevent = new TransformationEvent(
//                eventTime ?? DateTime.UtcNow.ToString("o"),
//                eventTimezoneOffset,
//                recordTime,
//                eventId,
//                inputEpcList,
//                inputQuantityList,
//                outputEpcList,
//                outputQuantityList,
//                transformationId,
//                bizStep,
//                disposition,
//                readPoint,
//                bizLocation,
//                businessTransactionList,
//                sourceList,
//                destinationList,
//                ilmd,
//                errorDeclaration);
//            _renderXmlDeclaration = renderXmlDeclaration;
//        }
//    }

//    public class RenderEPCISDocument : TemplateMixin
//    {
//        private readonly EPCISDocument _epcisDoc;
//        public EPCISDocumentEncoder Encoder { get; } = new EPCISDocumentEncoder();

//        public RenderEPCISDocument(
//            StandardBusinessDocumentHeader header = null,
//            List<ObjectEvent> objectEvents = null,
//            List<AggregationEvent> aggregationEvents = null,
//            List<TransactionEvent> transactionEvents = null,
//            List<TransformationEvent> transformationEvents = null,
//            bool renderXmlDeclaration = false,
//            string createdDate = null)
//        {
//            _epcisDoc = new EPCISDocument(
//                header,
//                objectEvents ?? new List<ObjectEvent>(),
//                aggregationEvents ?? new List<AggregationEvent>(),
//                transactionEvents ?? new List<TransactionEvent>(),
//                transformationEvents ?? new List<TransformationEvent>(),
//                renderXmlDeclaration,
//                createdDate ?? DateTime.UtcNow.ToString("o"));
//            _renderXmlDeclaration = renderXmlDeclaration;
//        }

//        public string Render(bool renderNamespaces = false, bool renderXmlDeclaration = false)
//        {
//            var context = new Dictionary<string, object>
//            {
//                { "header", _epcisDoc.Header },
//                { "object_events", _epcisDoc.ObjectEvents },
//                { "aggregation_events", _epcisDoc.AggregationEvents },
//                { "transaction_events", _epcisDoc.TransactionEvents },
//                { "transformation_events", _epcisDoc.TransformationEvents },
//                { "created_date", _epcisDoc.CreatedDate },
//                { "render_xml_declaration", _epcisDoc.RenderXmlDeclaration }
//            };

//            // This would use actual template rendering logic
//            return JsonConvert.SerializeObject(context, Formatting.Indented);
//        }
//    }

//    public class EPCISEventListDocument
//    {
//        private readonly EPCISDocument _epcisDoc;
//        private List<TemplateMixin> _templateEvents;
//        private bool _renderNamespaces;
//        private Dictionary<string, object> _additionalContext;

//        public EPCISDocumentEncoder _encoder { get; } = new EPCISDocumentEncoder();

//        public EPCISEventListDocument(
//            List<TemplateMixin> templateEvents,
//            StandardBusinessDocumentHeader header = null,
//            bool renderXmlDeclaration = true,
//            string createdDate = null,
//            bool renderNamespaces = false,
//            Dictionary<string, object> additionalContext = null)
//        {
//            _epcisDoc = new EPCISDocument(
//                header,
//                new List<ObjectEvent>(),
//                new List<AggregationEvent>(),
//                new List<TransactionEvent>(),
//                new List<TransformationEvent>(),
//                renderXmlDeclaration,
//                createdDate ?? DateTime.UtcNow.ToString("o"));
//            _templateEvents = templateEvents ?? throw new ArgumentNullException(nameof(templateEvents));
//            _renderNamespaces = renderNamespaces;
//            _additionalContext = additionalContext;
//        }

//        public List<TemplateMixin> TemplateEvents
//        {
//            get => _templateEvents;
//            set => _templateEvents = value ?? throw new ArgumentNullException(nameof(value));
//        }

//        public string Render()
//        {
//            // Move transformation events to their own list
//            var transformationEvents = new List<TransformationEvent>();
//            var otherEvents = new List<TemplateMixin>();

//            //foreach (var evt in _templateEvents)
//            //{
//            //    if (evt is TransformationEvent transformationEvent)
//            //    {
//            //        transformationEvents.Add(transformationEvent);
//            //    }
//            //    else
//            //    {
//            //        otherEvents.Add(evt);
//            //    }
//            //}

//            var context = new Dictionary<string, object>
//            {
//                { "header", _epcisDoc.Header },
//                { "template_events", otherEvents },
//                { "transformation_events", transformationEvents },
//                { "render_namespaces", _renderNamespaces },
//                { "render_xml_declaration", _epcisDoc.RenderXmlDeclaration },
//                { "created_date", _epcisDoc.CreatedDate },
//                { "additional_context", _additionalContext }
//            };

//            if (_additionalContext != null)
//            {
//                foreach (var kvp in _additionalContext)
//                {
//                    context.TryAdd(kvp.Key, kvp.Value);
//                }
//            }

//            // This would use actual template rendering logic
//            return JsonConvert.SerializeObject(context, Formatting.Indented);
//        }
//    }
//}
