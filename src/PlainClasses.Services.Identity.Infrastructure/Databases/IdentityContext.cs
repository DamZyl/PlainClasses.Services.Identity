using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PlainClasses.Services.Identity.Application.Configurations.Options;
using PlainClasses.Services.Identity.Domain.Models;

namespace PlainClasses.Services.Identity.Infrastructure.Databases
{
    public class IdentityContext: DbContext
    {
        private readonly IOptions<SqlOption> _sqlOption;
        
        #region DbSets
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonAuth> PersonAuths { get; set; }

        #endregion
        
        public IdentityContext(DbContextOptions options) : base(options) { }

        public IdentityContext(IOptions<SqlOption> sqlOption)
        {
            _sqlOption = sqlOption;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
        
            optionsBuilder.UseSqlServer(_sqlOption.Value.ConnectionString, 
                options => options.MigrationsAssembly("PlainClasses.Services.Identity.Api"));
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}