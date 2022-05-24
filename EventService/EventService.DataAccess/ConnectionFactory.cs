using HWA.GARDEN.Common.Data;
using HWA.GARDEN.Common.Security;
using HWA.GARDEN.Utilities.Validation;
using System.Data;
using System.Data.SqlClient;

namespace HWA.GARDEN.EventService.Data
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly ISecurityContext _context;

        public ConnectionFactory(ISecurityContext context)
        {
            Requires.NotNull(context, nameof(context));
            _context = context;
        }

        public IDbConnection GetConnection()
        {
            IDbConnection connection = new SqlConnection(_context.ConnectionString);
            connection.Open();
            return connection;
        }

        public IDbTransaction GetTransaction()
        {
            IDbConnection connection = GetConnection();
            return connection.BeginTransaction();
        }
    }
}
