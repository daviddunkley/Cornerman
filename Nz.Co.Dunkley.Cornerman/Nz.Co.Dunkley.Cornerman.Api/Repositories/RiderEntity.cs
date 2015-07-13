namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;

    public class RiderEntity : IEntity
    {
        public string Id { get; set; }
        public Guid RiderId { get; set; }
        public Guid MembershipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
    }
}