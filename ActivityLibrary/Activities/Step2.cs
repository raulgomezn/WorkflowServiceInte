using ActivityLibrary.Entities;
using ActivityLibrary.Transversal;
using NLog;
using System.Activities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace ActivityLibrary.Activities
{

    public sealed class Step2 : CodeActivity
    {
        // Defina un argumento de entrada de actividad
        public InArgument<WorkflowEntity> Entry { get; set; }
        public InArgument<List<ParkingEntity>> EntryParking { get; set; }
        public OutArgument<List<ParkingEntity>> Out { get; set; }
        /// <summary>
        /// El servicio que se necesita.
        /// </summary>
        static string service = "Place";
        static HttpClient client = new HttpClient();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Si la actividad devuelve un valor, se debe derivar de CodeActivity<TResult>
        // y devolver el valor desde el método Execute.
        protected override void Execute(CodeActivityContext context)
        {
            logger.Info("WorkflowInstanceId={0} Step2", context.WorkflowInstanceId);

            WorkflowEntity dataEntry = context.GetValue(this.Entry);
            List<ParkingEntity> dataEntryParking = context.GetValue(this.EntryParking);

            for (int i = 0; i < dataEntryParking.Count; i++)
            {
                string jObject = @"{""payload"": ""{\""{0}\"": " +
                    @"\""" + dataEntry.IdPlaces[i] + @"\""" +
                    @"}"" , ""deviceId"": """ +
                    service +
                    @"""}";
                logger.Info("Step2 jObject: {0}", jObject);

                logger.Info("Step2 Mapper- init");
                Mapper mapper = new Mapper();
                var result = mapper.Execute(jObject, new Foursquare());
                logger.Info("Step2 Mapper- final");

                float rating = float.Parse(result.Response.Venue.Rating.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." })) / float.Parse("2.0");
                dataEntryParking.ElementAt(i).Ranking = rating;
                logger.Info("Step2 google rating{0}", rating);
            }

            this.Out.Set(context, dataEntryParking);
        }
    }
}