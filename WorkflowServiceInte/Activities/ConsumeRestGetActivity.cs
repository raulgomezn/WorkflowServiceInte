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
            // Obtenga el valor de tiempo de ejecución del argumento de entrada Text
            string GmapKey = ConfigurationManager.AppSettings["GmapKey"];
            string BmapKey = ConfigurationManager.AppSettings["BmapKey"];

            WorkflowEntity dataEntry = context.GetValue(this.Data);




            /*CustomTrackingRecord record = new CustomTrackingRecord("MyRecord-Get-Init");
            record.Data.Add(new KeyValuePair<String, Object>("ExecutionTime", DateTime.Now));
            context.Track(record);*/
        }
    }
}
