namespace Nz.Co.Dunkley.Cornerman.Api.ApiControllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Models;
    using MongoDB.Bson;
    using Repositories;
    using Services;

    public class RideController : ApiController
    {
        private readonly IRideServices _rideServices;
        private readonly IRidePointRepository _ridePointRepository;

        public RideController(IRidePointRepository ridePointRepository = null, IRideServices rideServices = null)
        {
            _ridePointRepository = ridePointRepository ?? new RidePointRepository();
            _rideServices = rideServices ?? new RideServices();
        }

        [Route("api/rides")]
        public IHttpActionResult Post(Ride ride)
        {
            var returnedRide = _rideServices.Create(ride);

            return Ok(returnedRide);
        }

        [Route("api/ride/{rideId}")]
        public IHttpActionResult Get(ObjectId rideId)
        {
            var ride = _rideServices.Retrieve(rideId);

            return Ok(ride);
        }
    }
}
