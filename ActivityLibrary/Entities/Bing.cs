namespace ActivityLibrary.Entities
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class Bing
    {
        [JsonProperty("authenticationResultCode")]
        public string AuthenticationResultCode { get; set; }

        [JsonProperty("brandLogoUri")]
        public string BrandLogoUri { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("resourceSets")]
        public ResourceSet[] ResourceSets { get; set; }

        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }
    }

    public partial class ResourceSet
    {
        [JsonProperty("estimatedTotal")]
        public long EstimatedTotal { get; set; }

        [JsonProperty("resources")]
        public Resource[] Resources { get; set; }
    }

    public partial class Resource
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("destinations")]
        public Destination[] Destinations { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("origins")]
        public Destination[] Origins { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("destinationIndex")]
        public long DestinationIndex { get; set; }

        [JsonProperty("originIndex")]
        public long OriginIndex { get; set; }

        [JsonProperty("totalWalkDuration")]
        public long TotalWalkDuration { get; set; }

        [JsonProperty("travelDistance")]
        public double TravelDistance { get; set; }

        [JsonProperty("travelDuration")]
        public double TravelDuration { get; set; }
    }

    public partial class Destination
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }

    public partial class Bing
    {
        public static Bing FromJson(string json) => JsonConvert.DeserializeObject<Bing>(json, ConverterBing.Settings);
    }

    public static class SerializeBing
    {
        public static string ToJson(this Bing self) => JsonConvert.SerializeObject(self, ConverterBing.Settings);
    }

    public class ConverterBing
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}