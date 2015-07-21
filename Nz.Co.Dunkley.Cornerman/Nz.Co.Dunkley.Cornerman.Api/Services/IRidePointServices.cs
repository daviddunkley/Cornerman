namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRidePointServices
    {
        string Create(string riderId, RidePoint ridePoint);
        RidePoint Retrieve(string riderId, string ridePointId);
        List<RidePoint> RetrieveForRiderId(string riderId);
        void Delete(string riderId, string ridePointId);
        void DeleteForRiderId(string riderId);
    }
}
