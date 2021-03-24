﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using FlightRaderScrape.Json;
//
//    var radarArea = RadarArea.FromJson(jsonString);

namespace FlightRaderScrape.Json
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class RadarAreaClass
    {
        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public Total Total { get; set; }

        [JsonProperty("visible", NullValueHandling = NullValueHandling.Ignore)]
        public Total Visible { get; set; }
    }

    public partial class Total
    {
        [JsonProperty("ads-b", NullValueHandling = NullValueHandling.Ignore)]
        public long? AdsB { get; set; }

        [JsonProperty("mlat", NullValueHandling = NullValueHandling.Ignore)]
        public long? Mlat { get; set; }

        [JsonProperty("faa", NullValueHandling = NullValueHandling.Ignore)]
        public long? Faa { get; set; }

        [JsonProperty("flarm", NullValueHandling = NullValueHandling.Ignore)]
        public long? Flarm { get; set; }

        [JsonProperty("estimated", NullValueHandling = NullValueHandling.Ignore)]
        public long? Estimated { get; set; }

        [JsonProperty("satellite", NullValueHandling = NullValueHandling.Ignore)]
        public long? Satellite { get; set; }

        [JsonProperty("other", NullValueHandling = NullValueHandling.Ignore)]
        public long? Other { get; set; }
    }

    public partial struct RadarAreaElement
    {
        public double? Double;
        public string String;

        public static implicit operator RadarAreaElement(double Double) => new RadarAreaElement { Double = Double };
        public static implicit operator RadarAreaElement(string String) => new RadarAreaElement { String = String };
    }

    public partial struct RadarAreaValue
    {
        public List<RadarAreaElement> AnythingArray;
        public long? Integer;
        public RadarAreaClass RadarAreaClass;

        public static implicit operator RadarAreaValue(List<RadarAreaElement> AnythingArray) => new RadarAreaValue { AnythingArray = AnythingArray };
        public static implicit operator RadarAreaValue(long Integer) => new RadarAreaValue { Integer = Integer };
        public static implicit operator RadarAreaValue(RadarAreaClass RadarAreaClass) => new RadarAreaValue { RadarAreaClass = RadarAreaClass };
    }

    public class RadarArea
    {
        public static Dictionary<string, RadarAreaValue> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, RadarAreaValue>>(json, FlightRaderScrape.Json.ConverterBase.SettingsRadar);
    }


    internal class RadarAreaValueConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(RadarAreaValue) || t == typeof(RadarAreaValue?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new RadarAreaValue { Integer = integerValue };
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<RadarAreaClass>(reader);
                    return new RadarAreaValue { RadarAreaClass = objectValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<List<RadarAreaElement>>(reader);
                    return new RadarAreaValue { AnythingArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type RadarAreaValue");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (RadarAreaValue)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.AnythingArray != null)
            {
                serializer.Serialize(writer, value.AnythingArray);
                return;
            }
            if (value.RadarAreaClass != null)
            {
                serializer.Serialize(writer, value.RadarAreaClass);
                return;
            }
            throw new Exception("Cannot marshal type RadarAreaValue");
        }

        public static readonly RadarAreaValueConverter Singleton = new RadarAreaValueConverter();
    }

    internal class RadarAreaElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(RadarAreaElement) || t == typeof(RadarAreaElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                    var doubleValue = serializer.Deserialize<double>(reader);
                    return new RadarAreaElement { Double = doubleValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new RadarAreaElement { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type RadarAreaElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (RadarAreaElement)untypedValue;
            if (value.Double != null)
            {
                serializer.Serialize(writer, value.Double.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type RadarAreaElement");
        }

        public static readonly RadarAreaElementConverter Singleton = new RadarAreaElementConverter();
    }
}