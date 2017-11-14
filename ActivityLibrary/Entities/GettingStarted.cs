using Newtonsoft.Json;

namespace ActivityLibrary.Entities
{
    public partial class GettingStarted
    {
        [JsonProperty("sparql")]
        public Sparql Sparql { get; set; }

        [JsonProperty("?xml")]
        public Xml Xml { get; set; }
    }

    public partial class Xml
    {
        [JsonProperty("@version")]
        public string Version { get; set; }
    }

    public partial class Sparql
    {
        [JsonProperty("head")]
        public Head Head { get; set; }

        [JsonProperty("results")]
        public Results Results { get; set; }

        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
    }

    public partial class Results
    {
        [JsonProperty("result")]
        public Result[] Result { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("binding")]
        public Binding[] Binding { get; set; }
    }

    public partial class Binding
    {
        [JsonProperty("literal")]
        public string Literal { get; set; }

        [JsonProperty("@name")]
        public string Name { get; set; }
    }

    public partial class Head
    {
        [JsonProperty("variable")]
        public Variable[] Variable { get; set; }
    }

    public partial class Variable
    {
        [JsonProperty("@name")]
        public string Name { get; set; }
    }

    public partial class GettingStarted
    {
        public static GettingStarted FromJson(string json) => JsonConvert.DeserializeObject<GettingStarted>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GettingStarted self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}