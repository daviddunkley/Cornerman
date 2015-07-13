namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRidePointRepository
    {
        Task CreateOrUpdate(
            string riderId,
            DateTime pointDateTime,
            double altitude,
            double longitude,
            double latitude);

        Task<RidePoint> Retrieve(string riderId, DateTime pointDateTime);
        Task<List<RidePoint>> RetrieveForRiderId(string riderId);
        Task Delete(string riderId, DateTime pointDateTime);
    }
}
