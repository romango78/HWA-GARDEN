using HWA.GARDEN.Security;
using HWA.GARDEN.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HWA.GARDEN.EventService.Data
{
    public sealed class ConnectionFactory : BaseConnectionFactory
    {
        public ConnectionFactory(ISecurityContext context)
           : base(context)
        { }

        protected override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
