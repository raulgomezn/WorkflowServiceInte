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

namespace WorkflowServiceInte.Activities
{

    public sealed class ConsumeRestGetActivity : CodeActivity
    {
        // Defina un argumento de entrada de actividad de tipo string
        public InArgument<WorkflowEntity> Data { get; set; }

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

            //Consultar por las propertis hacer un get
            Log.Debug("Consultar propiedades");
            for (int i = 0; i < totalCoordinatesOrigins; i++)
            {
                //ParkingEntity entity = null;
                for (int j = 0; j < totalCoordinatesDestinatios; j++)
                {
                    //Consumir 
                    listEntity.Add(new ParkingEntity { }
                        );
                }
            }
            /*for (int i = 0; i < dataEntry.Properties.Length; i++)
            {
                ParkingEntity entity = null;
                switch (dataEntry.Properties[i].ToUpper())
                {
                    
                    case "GMAPS":
                        break;
                    case "BMAPS":
                        break;
                    case "GPLACES":
                        break;
                    case "FOURSQUARE":
                        break;
                    case "APIXU":
                        break;
                    case "OPENWEATHER":
                        break;
                    default:
                        break;
                }
            }*/



            CustomTrackingRecord record = new CustomTrackingRecord("MyRecord-Get-Init");
            record.Data.Add(new KeyValuePair<String, Object>("ExecutionTime", DateTime.Now));
            context.Track(record);
        }
    }



}
