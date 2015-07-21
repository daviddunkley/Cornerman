namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System.Collections.Generic;
    using Models;

    public interface IRideServices
    {
        string Upsert(Ride ride);
        string UpsertLead(string rideId, Rider lead);
        string UpsertTailgunner(string rideId, Rider tailgunner);
        string UpsertWingman(string rideId, Rider wingman);
        string UpsertRider(string rideId, Rider rider);
        Ride Retrieve(string rideId);
        List<Ride> RetrieveForMembershipId(string membershipId);
        Rider RetrieveLead(string rideId);
        Rider RetrieveTailgunner(string rideId);
        List<Rider> RetrieveWingmen(string rideId);
        List<Rider> RetrieveRiders(string rideId);
        void Delete(string rideId);
        void DeleteRider(string rideId, string riderId);
    }
}
