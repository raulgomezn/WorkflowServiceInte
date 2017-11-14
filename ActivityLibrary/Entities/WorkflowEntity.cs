using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivityLibrary.Entities
{
    public class WorkflowEntity
    {
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