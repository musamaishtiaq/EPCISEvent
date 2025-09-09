using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt
{
    public class JsonEncoder
    {
        private readonly JsonSerializerOptions _options;

        public JsonEncoder(bool indented = true)
        {
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = indented,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            // Enable polymorphism for event types
            _options.Converters.Add(new EpcisEventConverter());
        }

        public string Encode<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }

        public T Decode<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }
    }

    public class EpcisEventConverter : JsonConverter<EPCISEvent2>
    {
        public override EPCISEvent2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("type", out var typeProp))
                throw new JsonException("Missing 'type' discriminator for EPCIS event.");

            var type = typeProp.GetString();

            return type switch
            {
                "ObjectEvent" => JsonSerializer.Deserialize<ObjectEvent2>(root.GetRawText(), options),
                "AggregationEvent" => JsonSerializer.Deserialize<AggregationEvent2>(root.GetRawText(), options),
                "TransactionEvent" => JsonSerializer.Deserialize<TransactionEvent2>(root.GetRawText(), options),
                "TransformationEvent" => JsonSerializer.Deserialize<TransformationEvent2>(root.GetRawText(), options),
                _ => throw new JsonException($"Unknown EPCIS event type: {type}")
            };
        }

        public override void Write(Utf8JsonWriter writer, EPCISEvent2 value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case ObjectEvent2 objEvent:
                    JsonSerializer.Serialize(writer, objEvent, options);
                    break;
                case AggregationEvent2 aggEvent:
                    JsonSerializer.Serialize(writer, aggEvent, options);
                    break;
                case TransactionEvent2 txEvent:
                    JsonSerializer.Serialize(writer, txEvent, options);
                    break;
                case TransformationEvent2 trEvent:
                    JsonSerializer.Serialize(writer, trEvent, options);
                    break;
                default:
                    throw new JsonException($"Unsupported EPCIS event type: {value.GetType().Name}");
            }
        }
    }
}
