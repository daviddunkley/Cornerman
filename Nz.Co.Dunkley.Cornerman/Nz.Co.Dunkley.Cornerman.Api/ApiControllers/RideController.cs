namespace Nz.Co.Dunkley.Cornerman.Api.ApiControllers
{
    using System;
    using System.Web.Http;
    using Models;
    using MongoDB.Bson;
    using Newtonsoft.Json;
    using Repositories;
    using Services;

    public class RideController : ApiController
    {
        private readonly IRideServices _rideServices;
        private readonly IRidePointRepository _ridePointRepository;

        public RideController()
        {
            _ridePointRepository = new RidePointRepository();
            _rideServices = new RideServices();
        }

        public RideController(IRidePointRepository ridePointRepository = null, IRideServices rideServices = null)
        {
            _ridePointRepository = ridePointRepository ?? new RidePointRepository();
            _rideServices = rideServices ?? new RideServices();
        }

        [Route("api/rides")]
        [HttpPost]
        public IHttpActionResult Rides([FromBody]object request)
        {
            var ride = JsonConvert.DeserializeObject<Ride>(request.ToString());

            var returnedRide = _rideServices.Create(ride);

            return Ok();
        }

        [Route("api/ride/{rideId}")]
        public IHttpActionResult Get(string rideId)
        {
            var ride = _rideServices.Retrieve(rideId).Result;

            return Ok<Ride>(ride);
        }
    }
}
