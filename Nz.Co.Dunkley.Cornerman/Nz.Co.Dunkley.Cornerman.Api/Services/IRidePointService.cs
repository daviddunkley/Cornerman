namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRidePointService
    {
        Task Create(string riderId, RidePoint ridePoint);
        Task<List<RidePoint>> RetrieveForRiderId(string riderId);
    }
}
