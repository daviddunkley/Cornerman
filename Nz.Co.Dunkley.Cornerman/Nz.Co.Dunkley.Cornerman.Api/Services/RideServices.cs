namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Repositories;

    public class RideServices : IRideServices
    {
        private readonly IRideRepository _rideRepository;

        public RideServices(IRideRepository rideRepository = null)
        {
            _rideRepository = rideRepository ?? new RideRepositoryMongoDb();
        }

        public string Upsert(Ride ride)
        {
            var rideId = _rideRepository.Upsert(ride);

            return rideId;
        }

        public string UpsertLead(string rideId, Rider lead)
        {
            var riderId = _rideRepository.UpsertLead(rideId, lead);

            return riderId;
        }

        public string UpsertTailgunner(string rideId, Rider tailgunner)
        {
            var riderId = _rideRepository.UpsertTailgunner(rideId, tailgunner);

            return riderId;
        }

        public string UpsertWingman(string rideId, Rider wingman)
        {
            var riderId = _rideRepository.UpsertWingman(rideId, wingman);

            return riderId;
        }

        public string UpsertRider(string rideId, Rider rider)
        {
            var riderId = _rideRepository.UpsertRider(rideId, rider);

            return riderId;
        }

        public Ride Retrieve(string rideId)
        {
            var ride = _rideRepository.Retrieve(rideId);

            return ride;
        }

        public List<Ride> RetrieveForMembershipId(string membershipId)
        {
            var rides = _rideRepository.RetrieveForMembershipId(membershipId);

            return rides;
        }

        public Rider RetrieveLead(string rideId)
        {
            var ride = _rideRepository.Retrieve(rideId);

            return ride.Lead;
        }

        public Rider RetrieveTailgunner(string rideId)
        {
            var ride = _rideRepository.Retrieve(rideId);

            return ride.Tailgunner;
        }

        public List<Rider> RetrieveWingmen(string rideId)
        {
            var ride = _rideRepository.Retrieve(rideId);

            return ride.Wingmen;
        }

        public List<Rider> RetrieveRiders(string rideId)
        {
            var ride = _rideRepository.Retrieve(rideId);

            return ride.Riders;
        }

        public void Delete(string rideId)
        {
            _rideRepository.Delete(rideId);
        }

        public void DeleteRider(string rideId, string riderId)
        {
            throw new NotImplementedException();
        }
    }
}