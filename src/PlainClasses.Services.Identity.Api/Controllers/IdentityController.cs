using Microsoft.AspNetCore.Mvc;

namespace PlainClasses.Services.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        public IdentityController()
        {
        }

        [HttpGet]
        public string Get() => "Identity Service";
    }
}