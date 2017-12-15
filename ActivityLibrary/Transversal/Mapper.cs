using NLog;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ActivityLibrary.Transversal
{
    public class Mapper
    {
        private static HttpClient client = new HttpClient();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string urlClienteZookepper = "http://localhost:4321/zoo";

        /// <summary>
        /// Ejecuta la consulta.
        /// </summary>
        /// <param name="jObject">Objeto JSON</param>
        /// <returns>Variable dinamica.</returns>
        public dynamic Execute(string jObject, Object obj)
        {
            string result = ConsumePostAsync(urlClienteZookepper, jObject).Result;

            Object[] parameters = new Object[] { result };
            var data = obj.GetType().GetMethod("FromJson").Invoke(obj, parameters);
            
            return data;
        }
       
        /// <summary>
        /// Consumo del cliente de zookepper.
        /// </summary>
        /// <param name="url">Url del cliente.</param>
        /// <param name="jObject">Objeto json.</param>
        /// <returns>Json de respuesta.</returns>
        private static async Task<string> ConsumePostAsync(string url, string jObject)
        {
            logger.Info("Mapper ConsumePostAsync url{0} jObject{1}", url, jObject);

            string content = string.Empty;

            var stringContent = new StringContent(jObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, stringContent);

            if (response.StatusCode.ToString().ToUpper().Equals("OK"))
            {
                content = await response.Content.ReadAsStringAsync();
                logger.Info("Mapper StatusCode OK::: {0}", response.ToString());
            }
            else
            {
                logger.Info("Mapper StatusCode {0}::: {1}", response.StatusCode.ToString(), response.ToString());
            }

            return content;
        }
    }
}
