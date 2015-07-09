namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRidePointRepository
    {
        Task CreateOrUpdate(
            Guid riderId,
            DateTime pointDateTime,
            double altitude,
            double longitude,
            double latitude);

        Task<RidePoint> Retrieve(Guid riderId, DateTime pointDateTime);
        Task<List<RidePoint>> RetrieveForRiderId(Guid riderId);
    }
}
