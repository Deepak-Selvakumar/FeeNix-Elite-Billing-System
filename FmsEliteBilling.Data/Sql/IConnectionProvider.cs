using System.Data;

namespace FmsEliteBilling.Data.Oracle
{
    public interface IConnectionProvider
    {
        IDbConnection CreateConnection(string connectiontype); 
    }
}