using PlainClasses.Services.Identity.Application.Configurations.Dispatchers;

namespace PlainClasses.Services.Identity.Application.Commands
{
    public class LoginCommand : CommandBase<ReturnLoginViewModel>
    {
        public string PersonalNumber { get; set; }
        public string Password { get; set; }

        public LoginCommand(string personalNumber, string password)
        {
            PersonalNumber = personalNumber;
            Password = password;
        }
    }
}