﻿namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRidePointRepository
    {
        string CreateOrUpdate(
            string riderId,
            DateTime pointDateTime,
            double altitude,
            double longitude,
            double latitude);

        RidePoint Retrieve(string riderId, string ridePointId);
        List<RidePoint> RetrieveForRiderId(string riderId);
        void Delete(string riderId, string ridePointId);
        void DeleteForRiderId(string riderId);
    }
}
