using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkflowServiceInte.Entities
{
    public class Coordinates
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class DistanceMatrix
    {
        public decimal Distance { get; set; }
        public decimal Time { get; set; }
    }

    /// <summary>
    /// Entidad Parking
    /// </summary>
    public class ParkingEntity
    {
        public Coordinates Destination { get; set; }
        public DistanceMatrix DistanceMatrix { get; set; }
        public float Ranking { get; set; }
        public float Temperature { get; set; }

    }
}