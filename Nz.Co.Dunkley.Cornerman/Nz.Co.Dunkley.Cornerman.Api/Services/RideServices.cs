namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using MongoDB.Bson;
    using Repositories;

    public class RideServices : IRideServices
    {
        private readonly IRideRepository _rideRepository;

        public RideServices(IRideRepository rideRepository = null)
        {
            _rideRepository = rideRepository ?? new RideRepository();
        }

        public async Task Create(Ride ride)
        {
            await _rideRepository.Create(ride);
        }

        public async Task<Ride> Retrieve(string rideId)
        {
            var ride = await _rideRepository.Retrieve(rideId);

            return ride;
        }

        public async Task<List<Ride>> RetrieveForMembershipId(string membershipId)
        {
            var rides = await _rideRepository.RetrieveForMembershipId(membershipId);

            return rides;
        }

        public async Task Update(Ride ride)
        {
            await _rideRepository.Update(ride);
        }
    }
}