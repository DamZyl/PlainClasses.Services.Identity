using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlainClasses.Services.Identity.Application.Configurations.Validattion;

namespace PlainClasses.Services.Identity.Api.Configurations.Validations
{
    public class InvalidCommandProblemDetails : ProblemDetails
    {
        public InvalidCommandProblemDetails(InvalidCommandException exception)
        {
            Title = exception.Message;
            Status = StatusCodes.Status400BadRequest;
            Detail = exception.Details;
            Type = "https://somedomain/validation-error";
        }
    }
}