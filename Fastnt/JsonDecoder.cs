//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using EPCISEvent.Fastnt.CBV;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EPCISEvent.Fastnt
//{
//    public static class Decoder
//    {
//        public static List<QuantityElement> DecodeChildQuantityList(JToken childQuantityList)
//        {
//            var result = new List<QuantityElement>();

//            if (childQuantityList != null && childQuantityList.HasValues)
//            {
//                foreach (var item in childQuantityList)
//                {
//                    result.Add(new QuantityElement(
//                        item["epcClass"]?.ToString(),
//                        item["quantity"]?.ToObject<double?>(),
//                        item["uom"]?.ToString()
//                    ));
//                }
//            }

//            return result;
//        }

//        public static ErrorDeclaration DecodeErrorDeclaration(JToken errorDeclaration)
//        {
//            if (errorDeclaration == null || !errorDeclaration.HasValues)
//                return null;

//            return new ErrorDeclaration(
//                errorDeclaration["declarationTime"].ToObject<DateTime>(),
//                errorDeclaration["reason"]?.ToString(),
//                errorDeclaration["correctiveEventIDs"]?.ToObject<List<string>>()
//            );
//        }

//        public static List<Source> DecodeSourceList(JToken sourceList)
//        {
//            var result = new List<Source>();

//            if (sourceList != null && sourceList.HasValues)
//            {
//                foreach (var item in (JArray)sourceList)
//                {
//                    result.Add(new Source(
//                        (string)item["type"],
//                        (string)item["sourceValue"]
//                    ));
//                }
//            }

//            return result;
//        }

//        public static List<Destination> DecodeDestinationList(JToken destinationList)
//        {
//            var result = new List<Destination>();

//            if (destinationList != null && destinationList.HasValues)
//            {
//                foreach (var item in (JArray)destinationList)
//                {
//                    result.Add(new Destination(
//                        (string)item["type"],
//                        (string)item["destinationValue"]
//                    ));
//                }
//            }

//            return result;
//        }

//        public static List<BusinessTransaction> DecodeBusinessTransactionList(JToken bizTransactionList)
//        {
//            var result = new List<BusinessTransaction>();

//            if (bizTransactionList != null && bizTransactionList.HasValues)
//            {
//                foreach (var item in (JArray)bizTransactionList)
//                {
//                    result.Add(new BusinessTransaction(
//                        (string)item["type"],
//                        (string)item["bizTransaction"]
//                    ));
//                }
//            }

//            return result;
//        }

//        public static List<InstanceLotMasterDataAttribute> DecodeILMD(JToken ilmd)
//        {
//            var result = new List<InstanceLotMasterDataAttribute>();

//            if (ilmd != null && ilmd.HasValues)
//            {
//                foreach (var item in (JArray)ilmd)
//                {
//                    result.Add(new InstanceLotMasterDataAttribute(
//                        (string)item["name"],
//                        (string)item["value"]
//                    ));
//                }
//            }

//            return result;
//        }
//    }

//    /// <summary>
//    /// Base class for event decoders with common functionality
//    /// </summary>
//    public class BaseEventDecoder
//    {
//        public JObject _payload;

//        public BaseEventDecoder(string payload)
//        {
//            _payload = JObject.Parse(payload);
//        }

//        //public BaseEventDecoder(JObject payload)
//        //{
//        //    _payload = payload;
//        //}
//    }

//    /// <summary>
//    /// Decodes JSON into ObjectEvent instances
//    /// </summary>
//    public class ObjectEventDecoder : BaseEventDecoder
//    {
//        public JObject _eventData { get; private set; }

//        public ObjectEventDecoder(string jsonData) : base(jsonData)
//        {
//            try
//            {
//                var token = JToken.Parse(jsonData);

//                if (token.Type != JTokenType.Object)
//                    throw new JsonException("Expected JSON object but got " + token.Type);

//                _eventData = (JObject)token;
//            }
//            catch (JsonReaderException ex)
//            {
//                throw new JsonException("Invalid JSON format", ex);
//            }
//            catch (InvalidCastException ex)
//            {
//                throw new JsonException("Expected JSON object structure", ex);
//            }
//        }

//        //public ObjectEventDecoder(JObject jsonData) : base(jsonData)
//        //{
//        //    _eventData1 = _payload["objectEvent"] as JObject;
//        //}

//        public ObjectEvent GetEvent()
//        {
//            var objEvent = new ObjectEvent(
//                _eventData["eventTime"].ToString(),
//                _eventData["eventTimezoneOffset"]?.ToString(),
//                _eventData["recordTime"]?.ToString(),
//                _eventData["action"].ToString(),
//                _eventData["epcList"]?.ToObject<List<string>>() ?? new List<string>(),
//                _eventData["bizStep"]?.ToString(),
//                _eventData["disposition"]?.ToString(),
//                _eventData["readPoint"]?.ToString(),
//                _eventData["bizLocation"]?.ToString(),
//                _eventData["eventID"]?.ToString(),
//                Decoder.DecodeErrorDeclaration(_eventData["errorDeclaration"]),
//                Decoder.DecodeSourceList(_eventData["sourceList"]),
//                Decoder.DecodeDestinationList(_eventData["destinationList"]),
//                Decoder.DecodeBusinessTransactionList(_eventData["bizTransactionList"]),
//                Decoder.DecodeILMD(_eventData["ilmd"]),
//                Decoder.DecodeChildQuantityList(_eventData["quantityList"])
//            );

//            objEvent.Id = Guid.NewGuid().ToString();
//            return objEvent;
//        }
//    }

//    /// <summary>
//    /// Decodes JSON into AggregationEvent instances
//    /// </summary>
//    public class AggregationEventDecoder : BaseEventDecoder
//    {
//        private readonly JObject _eventData;

//        public AggregationEventDecoder(string payload) : base(payload)
//        {
//            _eventData = _payload["aggregationEvent"] as JObject;
//        }

//        //public AggregationEventDecoder(JObject payload) : base(payload)
//        //{
//        //    _eventData = _payload["aggregationEvent"] as JObject;
//        //}

//        public AggregationEvent GetEvent()
//        {
//            var aggEvent = new AggregationEvent(
//                _eventData["eventTime"].ToString(),
//                _eventData["eventTimezoneOffset"]?.ToString(),
//                _eventData["recordTime"]?.ToString(),
//                _eventData["action"].ToString(),
//                _eventData["parentID"]?.ToString(),
//                _eventData["childEPCs"]?.ToObject<List<string>>() ?? new List<string>(),
//                Decoder.DecodeChildQuantityList(_eventData["childQuantityList"]),
//                _eventData["bizStep"]?.ToString(),
//                _eventData["disposition"]?.ToString(),
//                _eventData["readPoint"]?.ToString(),
//                _eventData["bizLocation"]?.ToString(),
//                _eventData["eventID"]?.ToString(),
//                Decoder.DecodeErrorDeclaration(_eventData["errorDeclaration"]),
//                Decoder.DecodeSourceList(_eventData["sourceList"]),
//                Decoder.DecodeDestinationList(_eventData["destinationList"]),
//                Decoder.DecodeBusinessTransactionList(_eventData["bizTransactionList"])
//            );

//            aggEvent.Id = Guid.NewGuid().ToString();
//            return aggEvent;
//        }
//    }

//    /// <summary>
//    /// Decodes JSON into TransactionEvent instances
//    /// </summary>
//    public class TransactionEventDecoder : BaseEventDecoder
//    {
//        private readonly JObject _eventData;

//        public TransactionEventDecoder(string payload) : base(payload)
//        {
//            _eventData = _payload["transactionEvent"] as JObject;
//        }

//        //public TransactionEventDecoder(JObject payload) : base(payload)
//        //{
//        //    _eventData = _payload["transactionEvent"] as JObject;
//        //}

//        public TransactionEvent GetEvent()
//        {
//            var xactEvent = new TransactionEvent(
//                _eventData["eventTime"].ToString(),
//                _eventData["eventTimezoneOffset"]?.ToString(),
//                _eventData["recordTime"]?.ToString(),
//                _eventData["action"].ToString(),
//                _eventData["parentID"]?.ToString(),
//                _eventData["epcList"]?.ToObject<List<string>>() ?? new List<string>(),
//                _eventData["bizStep"]?.ToString(),
//                _eventData["disposition"]?.ToString(),
//                _eventData["readPoint"]?.ToString(),
//                _eventData["bizLocation"]?.ToString(),
//                _eventData["eventID"]?.ToString(),
//                Decoder.DecodeErrorDeclaration(_eventData["errorDeclaration"]),
//                Decoder.DecodeSourceList(_eventData["sourceList"]),
//                Decoder.DecodeDestinationList(_eventData["destinationList"]),
//                Decoder.DecodeBusinessTransactionList(_eventData["bizTransactionList"]),
//                Decoder.DecodeChildQuantityList(_eventData["quantityList"])
//            );

//            xactEvent.Id = Guid.NewGuid().ToString();
//            return xactEvent;
//        }
//    }
//}
