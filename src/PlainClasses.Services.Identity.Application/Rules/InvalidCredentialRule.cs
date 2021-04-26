using PlainClasses.Services.Identity.Application.Dto;
using PlainClasses.Services.Identity.Domain.SharedKernels;

namespace PlainClasses.Services.Identity.Application.Rules
{
    public class InvalidCredentialRule : IBusinessRule
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly PersonDto _person;
        private readonly string _requestPassword;

        public InvalidCredentialRule(IPasswordHasher passwordHasher, PersonDto person, string requestPassword)
        {
            _passwordHasher = passwordHasher;
            _person = person;
            _requestPassword = requestPassword;
        }

        public bool IsBroken() => !(_person != null && _passwordHasher.Check(_person.Password, _requestPassword));

        public string Message => "Invalid credentials.";
    }
}