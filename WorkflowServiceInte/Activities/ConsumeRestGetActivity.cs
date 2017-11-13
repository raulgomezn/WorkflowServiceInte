using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using LightCaseClient;
using System.Configuration;
using WorkflowServiceInte.Entities;
using NLog.Fluent;
using System.Activities.Tracking;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace WorkflowServiceInte.Activities
{

    public sealed class ConsumeRestGetActivity : CodeActivity
    {
        // Defina un argumento de entrada de actividad de tipo string
        public InArgument<WorkflowEntity> Data { get; set; }
        static HttpClient client = new HttpClient();

        // Si la actividad devuelve un valor, se debe derivar de CodeActivity<TResult>
        // y devolver el valor desde el método Execute.
        protected override void Execute(CodeActivityContext context)
        {
            Log.Debug("Inicio Actividad.");
            //// LLaves
            string GmapKey = ConfigurationManager.AppSettings["GmapKey"];
            string BmapKey = ConfigurationManager.AppSettings["BmapKey"];
            string GplacespKey = ConfigurationManager.AppSettings["GplacespKey"];
            string FoursquareId = ConfigurationManager.AppSettings["FoursquareId"];
            string FoursquareSecret = ConfigurationManager.AppSettings["FoursquareSecret"];
            string Openweathermap = ConfigurationManager.AppSettings["Openweathermap"];
            string Apixu = ConfigurationManager.AppSettings["Apixu"];

            WorkflowEntity dataEntry = context.GetValue(this.Data);

            List<ParkingEntity> listEntity = new List<ParkingEntity>();
            List<Service> items = new List<Service>();

            int totalCoordinatesOrigins = dataEntry.LatitudeOrigins.Length;
            int totalCoordinatesDestinatios = dataEntry.LatitudeDestinations.Length;

            ////Cargar la configuracion
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\" + "config.txt"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Service>>(json);
            }

            //-----
            //Tomar un solo servicio 
            items = items.GroupBy(c => c.methodValue, (key, c) => c.FirstOrDefault()).ToList();
            //y consultar los datos
            foreach (Service itemService in items)
            {
                switch (itemService.methodValue)
                {
                    case "BestPlace":
                        break;
                    case "BestRoad":
                        for (int i = 0; i < totalCoordinatesOrigins; i++)
                        {
                            for (int j = 0; j < totalCoordinatesDestinatios; j++)
                            {
                                string key = itemService.url.Contains("google") ?GmapKey:BmapKey;
                                string urlFinal=string.Format(itemService.url,
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
                        break;
                    case "BestWeather":
                        break;
                    default:
                        break;
                }
            }

            // devolver la lista final
            //-----


            CustomTrackingRecord record = new CustomTrackingRecord("MyRecord-Get-Init");
            record.Data.Add(new KeyValuePair<String, Object>("ExecutionTime", DateTime.Now));
            context.Track(record);
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
    }
}
