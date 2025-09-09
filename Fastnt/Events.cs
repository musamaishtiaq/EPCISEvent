using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt
{
    /// <summary>
    /// Helper enum for comparing and using event types
    /// </summary>
    public enum EventType
    {
        ObjectEvent,
        AggregationEvent,
        TransactionEvent,
        TransformationEvent
    }

    /// <summary>
    /// The Action type says how an event relates to the lifecycle of the entity
    /// being described. See section 7.3.2 of the EPCIS 1.2 standard.
    /// </summary>
    public enum Action
    {
        [EnumMember(Value = "ADD")]
        Add,

        [EnumMember(Value = "OBSERVE")]
        Observe,

        [EnumMember(Value = "DELETE")]
        Delete
    }

    /// <summary>
    /// A BusinessTransaction identifies a particular business transaction.
    /// As defined in section 7.3.5.3 of the protocol.
    /// </summary>
    public class BusinessTransaction
    {
        public string BizTransaction { get; set; }
        public string Type { get; set; }

        public BusinessTransaction()
        {
        }

        public BusinessTransaction(string bizTransaction, string type = null)
        {
            BizTransaction = bizTransaction;
            Type = type;
        }
    }

    /// <summary>
    /// Base class for Source/Destination types
    /// </summary>
    /// 

    public class InstanceLotMasterDataAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public InstanceLotMasterDataAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
    /// <summary>
    /// Base class for Source/Destination types
    /// </summary>
    /// 

    public class SourceDest
    {
        public string Type { get; set; }

        public SourceDest(string type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// Source information for business transfers
    /// </summary>
    public class Source : SourceDest
    {
        public string SourceValue { get; set; }

        public Source(string sourceType, string source) : base(sourceType)
        {
            SourceValue = source;
        }
    }

    /// <summary>
    /// Destination information for business transfers
    /// </summary>
    public class Destination : SourceDest
    {
        public string DestinationValue { get; set; }

        public Destination(string destinationType, string destination)
            : base(destinationType)
        {
            DestinationValue = destination;
        }
    }

    /// <summary>
    /// Error declaration information
    /// </summary>
    public class ErrorDeclaration
    {
        public DateTime DeclarationTime { get; set; }
        public string Reason { get; set; }
        public List<string> CorrectiveEventIds { get; set; }

        public ErrorDeclaration(
            DateTime declarationTime,
            string reason = null,
            List<string> correctiveEventIds = null)
        {
            DeclarationTime = declarationTime;
            Reason = reason;
            CorrectiveEventIds = correctiveEventIds ?? new List<string>();
        }
    }

    /// <summary>
    /// The EPCIS QuantityElement as outlined in section 7.3.3.3 of the protocol.
    /// </summary>
    public class QuantityElement
    {
        public string EpcClass { get; set; }
        public double? Quantity { get; set; }
        public string Uom { get; set; }

        public QuantityElement(string epcClass, double? quantity = null,
            string uom = null)
        {
            EpcClass = epcClass;
            Quantity = quantity;
            Uom = uom;
        }
    }

    public class EPCISDocument2
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "EPCISDocument";

        [JsonPropertyName("schemaVersion")]
        public string SchemaVersion { get; set; } = "2.0";

        [JsonPropertyName("creationDate")]
        public string CreationDate { get; set; } = DateTime.UtcNow.ToString();

        [JsonPropertyName("epcisHeader")]
        public EPCISHeader EpcisHeader { get; set; }

        [JsonPropertyName("epcisBody")]
        public EPCISBody EpcisBody { get; set; }

        [JsonPropertyName("@context")]
        public List<object> Context { get; set; }
    }

    public class EPCISHeader
    {
        [JsonPropertyName("epcisMasterData")]
        public EPCISMasterData EpcisMasterData { get; set; }
    }

    public class EPCISMasterData
    {
        [JsonPropertyName("vocabularyList")]
        public List<Vocabulary> VocabularyList { get; set; }
    }

    public class Vocabulary
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("vocabularyElementList")]
        public List<VocabularyElement> VocabularyElementList { get; set; }
    }

    public class VocabularyElement
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("attributes")]
        public List<VocabularyAttribute> Attributes { get; set; }
    }

    public class VocabularyAttribute
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("attribute")]
        public string Attribute { get; set; }
    }

    public class EPCISBody
    {
        [JsonPropertyName("eventList")]
        public List<EPCISEvent2> EventList { get; set; }
    }

    public abstract class EPCISEvent2
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("eventTime")]
        public string EventTime { get; set; }

        [JsonPropertyName("eventTimeZoneOffset")]
        public string EventTimeZoneOffset { get; set; }

        [JsonPropertyName("bizStep")]
        public string BizStep { get; set; }

        [JsonPropertyName("disposition")]
        public string Disposition { get; set; }

        [JsonPropertyName("readPoint")]
        public IdWrapper ReadPoint { get; set; }

        [JsonPropertyName("bizLocation")]
        public IdWrapper BizLocation { get; set; }

        [JsonPropertyName("bizTransactionList")]
        public List<BusinessTransaction> BizTransactionList { get; set; }

        [JsonPropertyName("sourceList")]
        public List<Source2> SourceList { get; set; }

        [JsonPropertyName("destinationList")]
        public List<Destination2> DestinationList { get; set; }
    }

    public class ErrorDeclaration2
    {
        public string DeclarationTime { get; set; }
        public string Reason { get; set; }
        public List<string> CorrectiveEventIds { get; set; }
    }

    public class ObjectEvent2 : EPCISEvent2
    {
        [JsonPropertyName("epcList")]
        public List<string> EpcList { get; set; }

        [JsonPropertyName("quantityList")]
        public List<QuantityElement> QuantityList { get; set; }

        [JsonPropertyName("ilmd")]
        public List<InstanceLotMasterDataAttribute> Ilmd { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }
    }

    public class AggregationEvent2 : EPCISEvent2
    {
        [JsonPropertyName("parentID")]
        public string ParentId { get; set; }

        [JsonPropertyName("childEPCs")]
        public List<string> ChildEpcs { get; set; }

        [JsonPropertyName("childQuantityList")]
        public List<QuantityElement> ChildQuantityList { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }
    }

    public class TransactionEvent2 : EPCISEvent2
    {
        [JsonPropertyName("parentID")]
        public string ParentId { get; set; }

        [JsonPropertyName("epcList")]
        public List<string> EpcList { get; set; }

        [JsonPropertyName("quantityList")]
        public List<QuantityElement> QuantityList { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }
    }

    public class TransformationEvent2 : EPCISEvent2
    {
        [JsonPropertyName("inputEPCList")]
        public List<string> InputEpcList { get; set; }

        [JsonPropertyName("inputQuantityList")]
        public List<QuantityElement> InputQuantityList { get; set; }

        [JsonPropertyName("outputEPCList")]
        public List<string> OutputEpcList { get; set; }

        [JsonPropertyName("outputQuantityList")]
        public List<QuantityElement> OutputQuantityList { get; set; }

        [JsonPropertyName("transformationID")]
        public string TransformationId { get; set; }

        [JsonPropertyName("certificationInfo")]
        public string CertificationInfo { get; set; }

        [JsonPropertyName("sensorElementList")]
        public List<SensorElement> SensorElementList { get; set; }

        [JsonPropertyName("persistentDisposition")]
        public PersistentDisposition PersistentDisposition { get; set; }
    }

    public class IdWrapper
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class Source2
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }
    }

    public class Destination2
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }
    }

    public class SensorElement
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("sensorMetadata")]
        public SensorMetadata SensorMetadata { get; set; }

        [JsonPropertyName("sensorReport")]
        public List<SensorReport> SensorReport { get; set; }
    }

    public class SensorMetadata
    {
        public string Time { get; set; }
        public string DeviceID { get; set; }
        public string DeviceMetadata { get; set; }
        public string RawData { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DataProcessingMethod { get; set; }
        public string BizRules { get; set; }
    }

    public class SensorReport
    {
        public string Type { get; set; }
        public string DeviceID { get; set; }
        public string RawData { get; set; }
        public string DataProcessingMethod { get; set; }
        public string Time { get; set; }
        public string Microorganism { get; set; }
        public string ChemicalSubstance { get; set; }
        public double? Value { get; set; }
        public string Component { get; set; }
        public string StringValue { get; set; }
        public bool? BooleanValue { get; set; }
        public string HexBinaryValue { get; set; }
        public string UriValue { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public double? MeanValue { get; set; }
        public double? PercRank { get; set; }
        public double? PercValue { get; set; }
        public string Uom { get; set; }
        public double? SDev { get; set; }
        public string DeviceMetadata { get; set; }
    }

    public class PersistentDisposition
    {
        public List<string> Set { get; set; }
        public List<string> Unset { get; set; }
    }
}
