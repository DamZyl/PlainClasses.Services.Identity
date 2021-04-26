using System.Data;

namespace PlainClasses.Services.Identity.Application.Configurations.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}