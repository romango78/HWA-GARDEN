using HWA.GARDEN.Common.Data;
using HWA.GARDEN.Common.Security;
using System.Data.Common;
using System.Data.SqlClient;

namespace HWA.GARDEN.CalendarService.Data
{
    public sealed class ConnectionFactory : BaseConnectionFactory
    {
        public ConnectionFactory(ISecurityContext context)
           : base(context)
        {}

        protected override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
