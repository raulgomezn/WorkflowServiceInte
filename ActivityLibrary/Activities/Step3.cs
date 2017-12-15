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
using ActivityLibrary.Transversal;

namespace ActivityLibrary.Activities
{

    public sealed class Step3 : CodeActivity
    {
        // Defina un argumento de entrada de actividad
        public InArgument<WorkflowEntity> Entry { get; set; }
        public InArgument<List<ParkingEntity>> EntryParking { get; set; }
        public OutArgument<List<ParkingEntity>> Out { get; set; }
        public OutArgument<ParkingEntity[]> OutFinal { get; set; }
        /// <summary>
        /// El servicio que se necesita.
        /// </summary>
        string service = "Weather";
        static HttpClient client = new HttpClient();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Si la actividad devuelve un valor, se debe derivar de CodeActivity<TResult>
        // y devolver el valor desde el método Execute.
        protected override void Execute(CodeActivityContext context)
        {
            logger.Info("WorkflowInstanceId={0} Step3", context.WorkflowInstanceId);

            WorkflowEntity dataEntry = context.GetValue(this.Entry);
            List<ParkingEntity> dataEntryParking = context.GetValue(this.EntryParking);

            for (int i = 0; i < dataEntryParking.Count; i++)
            {
                string jObject = @"{""payload"": ""{\""{0}\"": " +
                    dataEntry.LatitudeOrigins[i].ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." }) +
                    @",\""{1}\"":" +
                    dataEntry.LongitudeOrigins[i].ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." }) +
                    @"}"" , ""deviceId"": """ +
                    service +
                    @"""}";
                logger.Info("Step3 jObject: {0}", jObject);

                logger.Info("Step3 Mapper- init");
                Mapper mapper = new Mapper();
                var result = mapper.Execute(jObject, new Openweathermap());
                logger.Info("Step3 Mapper- final");

                float temp = float.Parse(result.Main.Temp.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." }));
                dataEntryParking.ElementAt(i).Temperature = temp;
                logger.Info("Step3 temp{0}", temp);
            }

            this.Out.Set(context, dataEntryParking);
            this.OutFinal.Set(context, dataEntryParking.ToArray());
        }
    }
}
