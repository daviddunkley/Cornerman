namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Repositories;

    public class RidePointServices : IRidePointServices
    {
        private readonly RidePointRepository _ridePointRepository;

        public RidePointServices(RidePointRepository ridePointRepository = null)
        {
            _ridePointRepository = ridePointRepository ?? new RidePointRepository();
        }

        public string Create(string riderId, RidePoint ridePoint)
        {
            var ridePointId = _ridePointRepository.CreateOrUpdate(
                riderId,
                ridePoint.PointDateTime,
                ridePoint.Altitude,
                ridePoint.Longitude,
                ridePoint.Latitude);

            return ridePointId;
        }

        public RidePoint Retrieve(string riderId, string ridePointId)
        {
            var ridePoint = _ridePointRepository.Retrieve(riderId, ridePointId);

            return ridePoint;
        }

        public List<RidePoint> RetrieveForRiderId(string riderId)
        {
            var ridePoints = _ridePointRepository.RetrieveForRiderId(riderId);

            return ridePoints;
        }

        public void Delete(string riderId, string ridePointId)
        {
            _ridePointRepository.Delete(riderId, ridePointId);
        }

        public void DeleteForRiderId(string riderId)
        {
            _ridePointRepository.DeleteForRiderId(riderId);
        }
    }
}