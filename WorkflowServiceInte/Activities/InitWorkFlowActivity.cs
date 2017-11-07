using Newtonsoft.Json;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WorkflowServiceInte.Entities;
using System.Linq;
using System.IO;
using System.Web.Script.Serialization;

namespace WorkflowServiceInte.Activities
{
    public sealed class InitWorkFlowActivity : CodeActivity
    {
        // Defina un argumento de entrada de actividad de tipo string
        public InArgument<string> Method { get; set; }
        public InArgument<Int32> MaximumSelected { get; set; }
        public OutArgument<Boolean> ResultProcess { get; set; }

        static HttpClient client = new HttpClient();

        protected override void Execute(CodeActivityContext context)
        {
            string method = context.GetValue(this.Method) == null ? "GET": context.GetValue(this.Method);
            Int32 maxSelected = context.GetValue(this.MaximumSelected) == 0 ? 1: context.GetValue(this.MaximumSelected);

            ////http://192.168.1.4:3030/parking/sparql
            Task<bool> result = ConsumeOwlAsync("http://localhost:3030/parking/sparql", method, maxSelected);

            this.ResultProcess.Set(context, result.Result);
        }

        static async Task<bool> ConsumeOwlAsync(string url, string method, Int32 maxSelected)
        {
            string finalMethod = string.Empty;

            switch (method.ToUpper().Trim())
            {
                case "GET":
                    finalMethod = "Read";
                    break;
                default:
                    finalMethod = "Read";
                    break;
            }

            string jObject = "query= PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>  PREFIX : <http://www.semanticweb.org/ca.mendoza968/ontologies/services#> SELECT ?URI ?methodValue (group_concat(?bodyLabel) as ?bodyLabels) (group_concat(?dataTypeLabel) as ?dataTypes) WHERE { ?service a :Service; :aboutProperty ?property. ?service :hasAPIURL ?URL; :hasMethod ?method . ?URL :hasStringValue ?URI.?method a :"+ finalMethod + "; :hasStringValue ?methodValue; :hasBodyField ?bodyField . ?bodyField rdfs:label ?bodyLabel; :hasDataType ?dataType . ?dataType rdfs:label ?dataTypeLabel .} GROUP BY ?URI ?methodValue";
            
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
                        foreach (var service in item)
                        {
                            distinctValues.Add(service);
                        }
                    }
                }

                try
                {
                    JavaScriptSerializer jsonSerialiser = new JavaScriptSerializer();
                    string json = jsonSerialiser.Serialize(distinctValues);

                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "config.txt", json);
                }
                catch (Exception)
                {
                    return false;
                }
                
                return true;
            }

            return false;
        }
    }
    public class Service
    {
        public string url { get; set; }
        public string methodValue { get; set; }
        public string dataTypes { get; set; }
        public string bodyLabels { get; set; }
    }
}
