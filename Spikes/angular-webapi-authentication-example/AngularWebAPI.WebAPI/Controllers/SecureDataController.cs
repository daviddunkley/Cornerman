using AngularWebAPI.WebAPI.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularWebAPI.WebAPI.Controllers
{
    using System.Web.Http.Cors;

    [Authorize]
    public class SecureDataController : ApiController
    {
        [EnableCors(origins: "http://localhost:2640", headers: "*", methods: "*")]
        public IHttpActionResult Get()
        {
            return Ok(new { secureData = "You have to be authenticated to access this!" });
        }
    }
}
