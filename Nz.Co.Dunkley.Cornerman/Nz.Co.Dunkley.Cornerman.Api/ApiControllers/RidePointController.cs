namespace Nz.Co.Dunkley.Cornerman.Api.ApiControllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Models;
    using Repositories;
    using Services;
    using Newtonsoft.Json;

    public class RidePointController : ApiController
    {
        private readonly IRidePointServices _ridePointServices;

        public RidePointController()
        {
            _ridePointServices = new RidePointServices();
        }

        public RidePointController(
            IRidePointRepository ridePointRepository = null, 
            IRidePointServices ridePointServices = null)
        {
            _ridePointServices = ridePointServices ?? new RidePointServices();
        }

        [Route("api/ride/{rideId}/ridePoint")]
        [HttpPost]
        public IHttpActionResult RidePoints(string rideId, [FromBody]object request)
        {
            var ridePoint = JsonConvert.DeserializeObject<RidePoint>(request.ToString());

            var ridePointId = _ridePointServices.Create(rideId, ridePoint);

            var locationUri = Request.RequestUri + "/" + ridePointId;

            return Created(locationUri, ridePoint);
        }

        [Route("api/ride/{rideId}/ridePoint/{ridePointId}")]
        [HttpGet]
        public IHttpActionResult Get(string rideId, string ridePointId)
        {
            var ridePoint = _ridePointServices.Retrieve(rideId, ridePointId);

            return Ok<RidePoint>(ridePoint);
        }

        [Route("api/ride/{rideId}/ridePoint")]
        [HttpGet]
        public IHttpActionResult GetForRideId(string rideId)
        {
            var ridePoints = _ridePointServices.RetrieveForRiderId(rideId);

            return Ok<List<RidePoint>>(ridePoints);
        }
    }
}
