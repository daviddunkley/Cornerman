﻿namespace Nz.Co.Dunkley.Cornerman.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using MongoDB.Bson;

    public interface IRideServices
    {
        Task Create(Ride ride);
        Task<Ride> Retrieve(string rideId);
        Task Update(Ride ride);
        Task<List<Ride>> RetrieveForMembershipId(string membershipId);
    }
}
