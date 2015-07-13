namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRideRepository
    {
        Task Create(Ride ride);
        Task<Ride> Retrieve(string rideId);
        Task<List<Ride>> RetrieveForMembershipId(string membershipId);
        Task Update(Ride ride);
        Task Delete(string rideId);
    }
}
