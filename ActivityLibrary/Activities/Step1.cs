using ActivityLibrary.Entities;
using ActivityLibrary.Transversal;
using NLog;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Linq;

namespace ActivityLibrary.Activities
{

    public sealed class Step1 : CodeActivity
    {
        // Defina un argumento de entrada de actividad
        public InArgument<WorkflowEntity> Entry { get; set; }
        public OutArgument<List<ParkingEntity>> Out { get; set; }
        /// <summary>
        /// El servicio que se necesita.
        /// </summary>
        static string service = "Road";

        static HttpClient client = new HttpClient();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Si la actividad devuelve un valor, se debe derivar de CodeActivity<TResult>
        // y devolver el valor desde el método Execute.
        protected override void Execute(CodeActivityContext context)
        {
            logger.Info("Step1 : WorkflowInstanceId={0}", context.WorkflowInstanceId);
 
            WorkflowEntity dataEntry = context.GetValue(this.Entry);

            int totalCoordinatesOrigins = dataEntry.LatitudeOrigins.Length;
            int totalCoordinatesDestinatios = dataEntry.LatitudeDestinations.Length;

            List<ParkingEntity> listEntity = new List<ParkingEntity>();

            for (int i = 0; i < totalCoordinatesOrigins; i++)
            {
                for (int j = 0; j < totalCoordinatesDestinatios; j++)
                {
                    string jObject = @"{""payload"": ""{\""{0}\"": " +
                        dataEntry.LatitudeOrigins[i].ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." }) +
                        @",\""{1}\"":" +
                        dataEntry.LongitudeOrigins[i].ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." }) +
                        @",\""{2}\"": " +
                        dataEntry.LatitudeDestinations[j].ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." }) +
                        @",\""{3}\"": " +
                        dataEntry.LongitudeDestinations[j].ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." }) +
                        @"}"" , ""deviceId"": """ +
                        service +
                        @"""}";

                    logger.Info("Step1 Mapper- init");
                    Mapper mapper = new Mapper();
                    var result = mapper.Execute(jObject, new GoogleMaps());
                    logger.Info("Step1 Mapper- final");
                    
                    Element resultEntity = result.Rows[0].Elements[0];
                    
                    decimal distance = Convert.ToDecimal(resultEntity.Distance.Value) /1000;
                    decimal time = Convert.ToDecimal(resultEntity.Duration.Value) / 60;
                    
                    logger.Info("Step1 distance{0}, time{1}", distance, time);

                    listEntity.Add(new ParkingEntity
                    {
                        Destination = new Coordinates
                        {
                            Latitude = dataEntry.LatitudeDestinations[j],
                            Longitude = dataEntry.LongitudeDestinations[j]
                        },
                        DistanceMatrix = new DistanceMatrix {
                            Distance = distance,
                            Time = time
                        }
                    });
                }
            }

            this.Out.Set(context, listEntity);
        }
    }
}
