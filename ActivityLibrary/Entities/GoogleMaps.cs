namespace ActivityLibrary.Entities
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class GoogleMaps
    {
        [JsonProperty("destination_addresses")]
        public string[] DestinationAddresses { get; set; }

        [JsonProperty("origin_addresses")]
        public string[] OriginAddresses { get; set; }

        [JsonProperty("rows")]
        public Row[] Rows { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Row
    {
        [JsonProperty("elements")]
        public Element[] Elements { get; set; }
    }

    public partial class Element
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }

        [JsonProperty("duration")]
        public Distance Duration { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Distance
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }

    public partial class GoogleMaps
    {
        public static GoogleMaps FromJson(string json) => JsonConvert.DeserializeObject<GoogleMaps>(json, ConverterGoogleMaps.Settings);
    }

    public static class SerializeGoogleMaps
    {
        public static string ToJson(this GoogleMaps self) => JsonConvert.SerializeObject(self, ConverterGoogleMaps.Settings);
    }

    public class ConverterGoogleMaps
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
