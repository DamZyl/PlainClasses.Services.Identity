using System;

namespace PlainClasses.Services.Identity.Application.Commands
{
    public class AuthDto
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string AuthName { get; set; }
    }
}