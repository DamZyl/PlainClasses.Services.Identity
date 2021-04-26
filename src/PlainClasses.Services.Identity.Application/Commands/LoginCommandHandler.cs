using System.Threading;
using System.Threading.Tasks;
using Dapper;
using PlainClasses.Services.Identity.Application.Configurations.Data;
using PlainClasses.Services.Identity.Application.Configurations.Dispatchers;
using PlainClasses.Services.Identity.Application.Dto;

namespace PlainClasses.Services.Identity.Application.Commands
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, ReturnLoginViewModel>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(ISqlConnectionFactory sqlConnectionFactory, IJwtHandler jwtHandler, IPasswordHasher passwordHasher)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
        }
        
        public async Task<ReturnLoginViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            
            const string sql = "SELECT " +
                               "[Person].[Id], " +
                               "[Person].[FirstName], " +
                               "[Person].[LastName], " +
                               "[Person].[PersonalNumber], " +
                               "[Person].[Password], " +
                               "[Person].[MilitaryRankAcr] " +
                               "FROM Persons AS [Person] " +
                               "WHERE [Person].[PersonalNumber] = @PersonalNumber ";
            
            var person = await connection.QuerySingleOrDefaultAsync<PersonDto>(sql, new { request.PersonalNumber });
            
            //ExceptionHelper.CheckRule(new PersonDoesNotExistRule(person));
            
            const string sqlAuths = "SELECT " +
                                    "[PersonAuth].[Id], " +
                                    "[PersonAuth].[PersonId], " +
                                    "[PersonAuth].[AuthName] " +
                                    "FROM PersonAuths AS [PersonAuth] " +
                                    "WHERE [PersonAuth].[PersonId] = @Id ";
            
            var auths = await connection.QueryAsync<AuthDto>(sqlAuths, new { person.Id });

            person.PersonAuths = auths.AsList();

            // ExceptionHelper.CheckRule(new InvalidCredentialRule(_passwordHasher, person, request.Password));
            
            return new ReturnLoginViewModel
                {
                    Token = _jwtHandler.CreateToken(person.Id,
                        $"{person.MilitaryRankAcr} {person.FirstName} {person.LastName}",
                        person.PersonAuths)
                };
        }
    }
}