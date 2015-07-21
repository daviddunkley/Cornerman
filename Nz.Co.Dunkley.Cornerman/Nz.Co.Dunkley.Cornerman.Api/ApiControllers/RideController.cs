namespace Nz.Co.Dunkley.Cornerman.Api.ApiControllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Models;
    using Newtonsoft.Json;
    using Services;

    public class RideController : ApiController
    {
        private readonly IRidePointServices _ridePointServices;
        private readonly IRideServices _rideServices;

        public RideController()
        {
            _rideServices = new RideServices();
            _ridePointServices = new RidePointServices();
        }

        public RideController(IRideServices rideServices = null, IRidePointServices ridePointServices = null)
        {
            _rideServices = rideServices ?? new RideServices();
            _ridePointServices = ridePointServices ?? new RidePointServices();
        }

        [Route("api/ride")]
        [HttpPost]
        public IHttpActionResult CreateRide([FromBody]object request)
        {
            var ride = JsonConvert.DeserializeObject<Ride>(request.ToString());
            
            var rideId = _rideServices.Upsert(ride);

            var locationUri = Request.RequestUri + "/" + rideId;

            return Created( locationUri, ride);
        }

        [Route("api/ride/{rideId}/tailgunner")]
        [HttpPost]
        public IHttpActionResult CreateTailGunner(string rideId, [FromBody]object request)
        {
            var rider = JsonConvert.DeserializeObject<Rider>(request.ToString());

            var riderId = _rideServices.UpsertTailgunner(rideId, rider);

            var locationUri = Request.RequestUri + "/" + riderId;

            return Created(locationUri, rider);
        }

        [Route("api/ride/{rideId}/wingmen")]
        [HttpPost]
        public IHttpActionResult CreateWingman(string rideId, [FromBody]object request)
        {
            var rider = JsonConvert.DeserializeObject<Rider>(request.ToString());

            var riderId = _rideServices.UpsertWingman(rideId, rider);

            var locationUri = Request.RequestUri + "/" + riderId;

            return Created(locationUri, rider);
        }

        [Route("api/ride/{rideId}/rider")]
        [HttpPost]
        public IHttpActionResult Rider(string rideId, [FromBody]object request)
        {
            var rider = JsonConvert.DeserializeObject<Rider>(request.ToString());

            var riderId = _rideServices.UpsertRider(rideId, rider);

            var locationUri = Request.RequestUri + "/" + riderId;

            return Created(locationUri, rider);
        }

        [Route("api/ride/{rideId}/rider/{riderId}/ridePoint")]
        [HttpPost]
        public IHttpActionResult Rides(string rideid, string riderId, [FromBody]object request)
        {
            var ridePoint = JsonConvert.DeserializeObject<RidePoint>(request.ToString());

            var ridePointId = _ridePointServices.Create(riderId, ridePoint);

            var locationUri = Request.RequestUri + "/" + ridePointId;

            return Created(locationUri, ridePoint);
        }

        [Route("api/ride/{rideId}")]
        [HttpGet]
        public IHttpActionResult Get(string rideId)
        {
            var ride = _rideServices.Retrieve(rideId);

            return Ok<Ride>(ride);
        }

        [Route("api/ride")]
        [HttpGet]
        public IHttpActionResult GetForMembershipId([FromUri] string membershipId)
        {
            var rides = _rideServices.RetrieveForMembershipId(membershipId);

            return Ok<List<Ride>>(rides);
        }

        [Route("api/ride/{rideId}")]
        [HttpDelete]
        public void Delete(string rideId)
        {
            _rideServices.Delete(rideId);
        }
    }
}
