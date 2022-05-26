using HWA.GARDEN.Common.Data;
using HWA.GARDEN.Common.Security;
using HWA.GARDEN.Utilities.Validation;
using System.Data.Common;
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

        public DbConnection GetConnection()
        {
            DbConnection connection = new SqlConnection(_context.ConnectionString);
            connection.Open();
            return connection;
        }

        public DbTransaction GetTransaction()
        {
            DbConnection connection = GetConnection();
            return connection.BeginTransaction();
        }
    }
}
