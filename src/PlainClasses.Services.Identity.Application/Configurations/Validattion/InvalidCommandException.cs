using System;

namespace PlainClasses.Services.Identity.Application.Configurations.Validattion
{
    public class InvalidCommandException : Exception
    {
        public string Details { get; }
        public InvalidCommandException(string message, string details) : base(message)
        {
            Details = details;
        }
    }
}