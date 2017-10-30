using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkflowServiceInte.Entities
{
    public class WorkflowEntity
    {
        /// <summary>
        /// Campo Url
        /// </summary>
        private string[] urls;

        /// <summary>
        /// Propiedad Url
        /// </summary>
        public string[] Urls
        {
            get { return urls; }
            set { urls = value; }
        }

        /// <summary>
        /// Campo Method
        /// </summary>
        private string[] method;

        /// <summary>
        /// Propiedad Method
        /// </summary>
        public string[] Method
        {
            get { return method; }
            set { method = value; }
        }

        /// <summary>
        /// Campo Body
        /// </summary>
        private string[] body;

        /// <summary>
        /// Propiedad Body
        /// </summary>
        public string[] Body
        {
            get { return body; }
            set { body = value; }
        }

        /// <summary>
        /// Campo properties (Gmaps, Bmaps etc)
        /// </summary>
        private string[] properties;

        /// <summary>
        /// Propiedad properties
        /// </summary>
        public string[] Properties
        {
            get { return properties; }
            set { properties = value; }
        }

        /// <summary>
        /// Campo latOrigins
        /// </summary>
        private Decimal[] latOrigins;

        /// <summary>
        /// Propiedad LatiduteOrigins
        /// </summary>
        public Decimal[] LatitudeOrigins
        {
            get { return latOrigins; }
            set { latOrigins = value; }
        }

        /// <summary>
        /// Campo longitudeOrigins
        /// </summary>
        private Decimal[] longitudeOrigins;

        /// <summary>
        /// Propiedad LongitudeOrigins
        /// </summary>
        public Decimal[] LongitudeOrigins
        {
            get { return longitudeOrigins; }
            set { longitudeOrigins = value; }
        }

        /// <summary>
        /// Campo latiduteDestinations
        /// </summary>
        private Decimal[] latitudeDestinations;

        /// <summary>
        /// Propiedad LatiduteDestinations
        /// </summary>
        public Decimal[] LatitudeDestinations
        {
            get { return latitudeDestinations; }
            set { latitudeDestinations = value; }
        }

        /// <summary>
        /// Campo  LongitudeDestinations
        /// </summary>
        private Decimal[] longitudeDestinations;

        /// <summary>
        /// Propiedad LongitudeDestinations
        /// </summary>
        public Decimal[] LongitudeDestinations
        {
            get { return longitudeDestinations; }
            set { longitudeDestinations = value; }
        }

        /// <summary>
        /// Campo IdPlaces
        /// </summary>
        private string[] idPlaces;

        /// <summary>
        /// Propiedad IdPlaces
        /// </summary>
        public string[] IdPlaces
        {
            get { return idPlaces; }
            set { idPlaces = value; }
        }
    }
}