namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using MongoDB.Bson;

    public interface IRideServices
    {
        Ride Create(Ride ride);
        Ride Retrieve(ObjectId rideId);
        Ride Update(Ride ride);
        List<Ride> RetrieveForMembershipId(Guid membershipId);
    }
}
