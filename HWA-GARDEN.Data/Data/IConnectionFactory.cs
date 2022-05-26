using System.Data.Common;

namespace HWA.GARDEN.Common.Data
{
    public interface IConnectionFactory
    {
        public DbConnection GetConnection();

        public DbTransaction GetTransaction();
    }
}
