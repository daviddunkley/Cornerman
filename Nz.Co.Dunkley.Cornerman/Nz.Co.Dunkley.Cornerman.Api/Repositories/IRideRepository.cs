namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRideRepository
    {
        string Upsert(Ride ride);
        string UpsertLead(string rideId, Rider lead);
        string UpsertTailgunner(string rideId, Rider tailgunner);
        string UpsertWingman(string rideId, Rider wingman);
        string UpsertRider(string rideId, Rider rider);
        Ride Retrieve(string rideId);
        List<Ride> RetrieveForMembershipId(string membershipId); 
        void Delete(string rideId);
        void DeleteRider(string rideId, string riderId);
    }
}
