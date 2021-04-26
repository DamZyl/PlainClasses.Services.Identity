using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlainClasses.Services.Identity.Application.Commands;

namespace PlainClasses.Services.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ReturnLoginViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Login([FromBody]LoginRequest request) 
        {
            var token = await _mediator.Send(new LoginCommand(request.PersonalNumber, request.Password));

            return Created(string.Empty, token.Token);
        }
    }
}