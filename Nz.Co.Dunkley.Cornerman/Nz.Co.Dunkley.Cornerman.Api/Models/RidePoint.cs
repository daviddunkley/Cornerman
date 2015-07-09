﻿namespace Nz.Co.Dunkley.Cornerman.Api.Models
{
    using System;

    public class RidePoint
    {
        public Guid Guid { get; set; }
        public DateTime PointDateTime { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
    }
}