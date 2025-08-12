using Microsoft.Extensions.Configuration;
using FmsEliteBilling.Data.BusinessLayer;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace FmsEliteBilling.Data.Oracle
{
    public class ConnectionProvider(IConfiguration configuration, AESSecurity_BLL security) : IConnectionProvider
    {


        private readonly IConfiguration _configuration = configuration;
        private string _connectionString;


        private readonly AESSecurity_BLL _security = security;

        public IDbConnection CreateConnection(string connectiontype)
        {
            var data = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string clientId = _configuration["OpenAPIClientId"].Replace("-", "");
            string DecryptString;
            switch (connectiontype) 
            {
            case ConnectionStringConfig.FeeDB:
                DecryptString = _configuration.GetSection("ConnectionStrings").GetSection("FEEDB").Value;
                break; 
            default:
                DecryptString = _configuration.GetSection("ConnectionStrings").GetSection("SELDB").Value;
                break;
            }
            string FinalValue = Regex.Unescape(_security.Decrypt(DecryptString, clientId));
            _connectionString = FinalValue;
            return new SqlConnection(_connectionString);
        }

    }

}