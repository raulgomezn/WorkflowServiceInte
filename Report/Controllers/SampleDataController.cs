using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NJsonSchema.CodeGeneration.CSharp;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Report.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        static HttpClient client = new HttpClient();

        [HttpGet("[action]")]
        public List<Service> WeatherForecasts()
        {
            List<Service> resultUrl = ConsumeOwlAsync("http://localhost:3030/virtual/sparql").Result;

            return resultUrl;
        }

        [HttpPost("[action]")]
        public IActionResult WeatherForecasts([FromBody] Service entidad)
        {
            string urlfinal = string.Empty;
            urlfinal = formatUrl(entidad.url);

            string json = ConsumeGetAsync(urlfinal).Result;

            NJsonSchema.JsonSchema4 schema = NJsonSchema.JsonSchema4.FromSampleJson(json);
            string jsonSchema = schema.ToJson();

            CSharpGeneratorSettings settings = new CSharpGeneratorSettings
            {
                Namespace = "ActivityLibrary.Entities",
                ClassStyle = CSharpClassStyle.Poco
            };

            var generator = new CSharpGenerator(schema, settings);

            string file = generator.GenerateFile();

            file = file.Replace(@"[System.CodeDom.Compiler.GeneratedCode(""NJsonSchema"", ""9.10.15.0(Newtonsoft.Json v10.0.0.0)"")]", "");
            file = file.Replace(" : System.ComponentModel.INotifyPropertyChanged", "");

            var contentType = "text/plain";
            HttpContext.Response.ContentType = contentType;

            FileContentResult result = new FileContentResult(Encoding.ASCII.GetBytes(file), contentType)
            {
                FileDownloadName = "class.cs"
            };

            return Ok(file);
        }

        private static string formatUrl(string urlfinal)
        {
            int count = 0;
            foreach (char item in urlfinal)
            {
                if (item.Equals('{'))
                    count++;
            }

            switch (count)
            {
                case 1:
                    string idPlace;
                    if (urlfinal.Contains("google"))
                        idPlace = "ChIJHXdiL6OZP44RaG3andYR6fs";
                    else
                        idPlace = "4e2867dfb0fba988bb6b98a8";
                    urlfinal = string.Format(urlfinal, idPlace);
                    break;
                case 2:
                    urlfinal = string.Format(urlfinal, "4.603062", "-74.065258");
                    break;
                case 4:
                    urlfinal = string.Format(urlfinal, "4.682487", "-74.120194", "4.603062", "-74.065258");
                    break;
                default:
                    break;
            }

            return urlfinal;
        }

        static async Task<List<Service>> ConsumeOwlAsync(string url)
        {
            string content = string.Empty;

            string jObject = "query= PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>PREFIX : <http://www.semanticweb.org/ca.mendoza968/ontologies/services#>SELECT ?URI ?methodValue (group_concat(?bodyLabel) as ?bodyLabels) (group_concat(?dataTypeLabel) as ?dataTypes)WHERE { ?service a :Service; :aboutProperty ?property . ?service :hasAPIURL ?URL; :hasMethod ?method . ?URL :hasStringValue ?URI . ?method a :Read; :hasStringValue ?methodValue; :hasBodyField ?bodyField . ?bodyField rdfs:label ?bodyLabel; :hasDataType ?dataType . ?dataType rdfs:label ?dataTypeLabel .}GROUP BY ?URI ?methodValue";

            var stringContent = new StringContent(jObject, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = await client.PostAsync(url, stringContent);

            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }

            List<Service> listServices = new List<Service>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);

            GettingStarted data = GettingStarted.FromJson(JsonConvert.SerializeXmlNode(xmlDoc));

            foreach (Result item in data.Sparql.Results.Result)
            {
                Service itemService = new Service();

                foreach (Binding detail in item.Binding)
                {
                    if (detail.Name.Equals("URI"))
                    {
                        itemService.url = detail.Literal;
                    }

                    if (detail.Name.Equals("methodValue"))
                    {
                        itemService.methodValue = detail.Literal;
                    }

                    if (detail.Name.Equals("dataTypes"))
                    {
                        itemService.dataTypes = detail.Literal;
                    }

                    if (detail.Name.Equals("bodyLabels"))
                    {
                        itemService.bodyLabels = detail.Literal;
                    }

                    itemService.method = "GET";
                }

                listServices.Add(itemService);
            }

            return listServices;
        }

        static async Task<string> ConsumeGetAsync(string url)
        {
            string content = string.Empty;

            HttpResponseMessage response = await client.GetAsync("https://" + url);

            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }
            return content;
        }
    }

    public class Service
    {
        public string url { get; set; }
        public string methodValue { get; set; }
        public string dataTypes { get; set; }
        public string bodyLabels { get; set; }
        public string method { get; set; }
    }
}
