using MicroserviceLibrary.Application.Configurations.Options;
using MicroserviceLibrary.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PlainClasses.Services.Identity.Domain.Models;

namespace PlainClasses.Services.Identity.Infrastructure.Databases
{
    public class IdentityContext: AbstractApplicationDbContext
    {
        #region DbSets
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonAuth> PersonAuths { get; set; }

        #endregion
        
        public IdentityContext(IOptions<SqlOption> options) : base(options, "PlainClasses.Services.Identity.Api") { }
    }
}