using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace CineList.Infrastructure.Context
{

    public class DapperContext
    {
        private readonly IConfiguration _configuration;

        public DapperContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {

            return new NpgsqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
