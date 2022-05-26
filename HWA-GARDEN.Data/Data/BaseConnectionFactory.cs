using HWA.GARDEN.Common.Security;
using HWA.GARDEN.Utilities.Validation;
using System.Data.Common;

namespace HWA.GARDEN.Common.Data
{
    public abstract class BaseConnectionFactory : IConnectionFactory
    {
        private readonly ISecurityContext _context;

        public BaseConnectionFactory(ISecurityContext context)
        {
            Requires.NotNull(context, nameof(context));
            _context = context;
        }

        public DbConnection GetConnection()
        {
            DbConnection connection = CreateConnection(_context.ConnectionString);
            connection.Open();
            return connection;
        }

        public DbTransaction GetTransaction()
        {
            DbConnection connection = GetConnection();
            return connection.BeginTransaction();
        }

        protected abstract DbConnection CreateConnection(string connectionString);
    }
}
