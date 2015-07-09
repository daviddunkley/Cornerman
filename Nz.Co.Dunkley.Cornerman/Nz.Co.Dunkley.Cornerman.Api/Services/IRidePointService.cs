namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRidePointService
    {
        Task Create(Guid riderId, RidePoint ridePoint);
        Task<List<RidePoint>> RetrieveForRiderId(Guid riderId);
    }
}
