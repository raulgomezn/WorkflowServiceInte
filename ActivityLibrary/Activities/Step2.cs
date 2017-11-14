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

namespace ActivityLibrary.Activities
{

    public sealed class Step2 : CodeActivity
    {
        // Defina un argumento de entrada de actividad
        public InArgument<WorkflowEntity> Entry { get; set; }
        public OutArgument<List<ParkingEntity>> Out { get; set; }
        static HttpClient client = new HttpClient();

        // Si la actividad devuelve un valor, se debe derivar de CodeActivity<TResult>
        // y devolver el valor desde el método Execute.
        protected override void Execute(CodeActivityContext context)
        {
            string method = "Read";
            Int32 maxSelected = 1;
            string service = "BestRoad";
            string GmapKey = ConfigurationManager.AppSettings["GmapKey"];
            string BmapKey = ConfigurationManager.AppSettings["BmapKey"];

            WorkflowEntity dataEntry = context.GetValue(this.Entry);

            ////http://192.168.1.4:3030/parking/sparql
            string resultUrl = ConsumeOwlAsync("http://localhost:3030/parking/sparql", method, maxSelected, service).Result;

            int totalCoordinatesOrigins = dataEntry.LatitudeOrigins.Length;
            int totalCoordinatesDestinatios = dataEntry.LatitudeDestinations.Length;

            List<ParkingEntity> listEntity = new List<ParkingEntity>();

            for (int i = 0; i < totalCoordinatesOrigins; i++)
            {
                for (int j = 0; j < totalCoordinatesDestinatios; j++)
                {
                    string key = resultUrl.Contains("google") ? GmapKey : BmapKey;

                    string urlFinal = string.Format(resultUrl,
                        dataEntry.LatitudeOrigins[i], dataEntry.LongitudeOrigins[i],
                        dataEntry.LatitudeDestinations[j], dataEntry.LongitudeDestinations[j],
                        "driving", key);

                    Task<string> result = ConsumeGetAsync(urlFinal);

                    listEntity.Add(new ParkingEntity
                    {
                        Destination = new Coordinates
                        {
                            Latitude = dataEntry.LatitudeDestinations[j],
                            Longitude = dataEntry.LongitudeDestinations[j]
                        }
                    });
                }
            }


            this.Out.Set(context, listEntity);
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
                    return distinctValues.Select(x=> x.url).Single();
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
