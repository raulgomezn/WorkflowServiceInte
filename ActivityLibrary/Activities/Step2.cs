using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using ActivityLibrary.Entities;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Configuration;
using NLog;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace ActivityLibrary.Activities
{

    public sealed class Step2 : CodeActivity
    {
        // Defina un argumento de entrada de actividad
        public InArgument<WorkflowEntity> Entry { get; set; }
        public InArgument<List<ParkingEntity>> EntryParking { get; set; }
        public OutArgument<List<ParkingEntity>> Out { get; set; }
        static HttpClient client = new HttpClient();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Si la actividad devuelve un valor, se debe derivar de CodeActivity<TResult>
        // y devolver el valor desde el método Execute.
        protected override void Execute(CodeActivityContext context)
        {
            logger.Info("WorkflowInstanceId={0} Step2", context.WorkflowInstanceId);
            string method = "Read";
            Int32 maxSelected = 1;
            string service = "BestPlace";
            string GplacespKey = ConfigurationManager.AppSettings["GplacespKey"];
            string FoursquareId = ConfigurationManager.AppSettings["FoursquareId"];
            string FoursquareSecret = ConfigurationManager.AppSettings["FoursquareSecret"];

            WorkflowEntity dataEntry = context.GetValue(this.Entry);
            List<ParkingEntity> dataEntryParking = context.GetValue(this.EntryParking);

            ////http://192.168.1.4:3030/parking/sparql
            string resultUrl = ConsumeOwlAsync("http://localhost:3030/parking/sparql", method, maxSelected, service).Result;

            int count = 0;

            foreach (ParkingEntity item in dataEntryParking)
            {
                string urlFinal = string.Empty;

                if (resultUrl.Contains("google"))
                {
                    //https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key={1}
                    string key = GplacespKey;
                    urlFinal = string.Format(resultUrl, dataEntry.IdPlaces[count], key);

                    string result = ConsumeGetAsync(urlFinal).Result;

                    var data = JObject.Parse(result);

                    float rating = float.Parse("0.0");

                    item.Ranking = rating;
                }
                else
                {
                    string urlfinal = "https://api.foursquare.com/v2/venues/{0}?&v=20161016&client_id={1}&client_secret={2}";
                    urlFinal = string.Format(urlfinal, dataEntry.IdPlaces[count].ToString(), FoursquareId, FoursquareSecret);

                    string result = ConsumeGetAsync(urlFinal).Result;

                    var data = JObject.Parse(result);
                    string value = (string)data["response"]["venue"]["rating"];
                    float rating = float.Parse(value.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." })) / float.Parse("2.0");

                    item.Ranking = rating;
                }

                count++;
            }

            this.Out.Set(context, dataEntryParking);
        }

        static async Task<string> ConsumeGetAsync(string url)
        {
            string content = string.Empty;

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }
            return content;
        }

        static async Task<string> ConsumeOwlAsync(string url, string method, Int32 maxSelected, string service)
        {
            string finalMethod = method;
            string filter = string.Empty;

            if (!string.IsNullOrEmpty(service) || !service.Contains("*"))
                filter = string.Format("FILTER (?methodValue = \"{0}\") .", service);

            string jObject = "query= PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>  PREFIX : <http://www.semanticweb.org/ca.mendoza968/ontologies/services#> SELECT ?URI ?methodValue (group_concat(?bodyLabel) as ?bodyLabels) (group_concat(?dataTypeLabel) as ?dataTypes) WHERE { ?service a :Service; :aboutProperty ?property. ?service :hasAPIURL ?URL; :hasMethod ?method . ?URL :hasStringValue ?URI.?method a :" + finalMethod + "; :hasStringValue ?methodValue; :hasBodyField ?bodyField . ?bodyField rdfs:label ?bodyLabel; :hasDataType ?dataType . ?dataType rdfs:label ?dataTypeLabel . " + filter + "} GROUP BY ?URI ?methodValue";

            var stringContent = new StringContent(jObject, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = await client.PostAsync(url, stringContent);

            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

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
                    }

                    listServices.Add(itemService);
                }

                List<Service> distinctValues = new List<Service>();

                if (maxSelected == 1)
                {
                    distinctValues = listServices.GroupBy(c => c.methodValue, (key, c) => c.FirstOrDefault()).ToList();
                }
                else
                {
                    var finalTrans = listServices.GroupBy(c => c.methodValue, (key, c) => c.Take(maxSelected));

                    ////Convertir de Ienumerable a List.
                    foreach (var item in finalTrans)
                    {
                        foreach (var serviceItem in item)
                        {
                            distinctValues.Add(serviceItem);
                        }
                    }
                }

                try
                {
                    return distinctValues.Select(x => x.url).Single();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }
    }
}