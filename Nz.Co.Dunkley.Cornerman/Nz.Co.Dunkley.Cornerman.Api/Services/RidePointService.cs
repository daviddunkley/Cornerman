namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Repositories;

    public class RidePointService : IRidePointService
    {
        private readonly RidePointRepository _ridePointRepository;

        public RidePointService(RidePointRepository ridePointRepository = null)
        {
            _ridePointRepository = ridePointRepository ?? new RidePointRepository();
        }

        public async Task Create(string riderId, RidePoint ridePoint)
        {
            await _ridePointRepository.CreateOrUpdate(
                riderId,
                ridePoint.PointDateTime,
                ridePoint.Altitude,
                ridePoint.Longitude,
                ridePoint.Latitude);
        }

        public async Task<List<RidePoint>> RetrieveForRiderId(string riderId)
        {
            var ridePoints = await _ridePointRepository.RetrieveForRiderId(riderId);

            return ridePoints;
        }
    }
}