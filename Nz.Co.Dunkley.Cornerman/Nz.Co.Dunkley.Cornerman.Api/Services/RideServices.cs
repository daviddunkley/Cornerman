namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using MongoDB.Bson;
    using MongoRepository;
    using Repositories;

    public class RideServices : IRideServices
    {
        private readonly MongoRepository<Ride> _rideRepository;

        public RideServices(
            MongoRepository<Ride> rideRepository = null)
        {
            _rideRepository = rideRepository ?? new MongoRepository<Ride>();
        }

        public Ride Create(Ride ride)
        {
            return _rideRepository.Add(ride);
        }

        public Ride Retrieve(ObjectId rideId)
        {
            var ride = _rideRepository.GetById(rideId);

            return ride;
        }

        public List<Ride> RetrieveForMembershipId(Guid membershipId)
        {
            throw new NotImplementedException();
        }

        public Ride Update(Ride ride)
        {
            return _rideRepository.Update(ride);
        }
    }
}