using System.Data.Common;

namespace HWA.GARDEN.Data
{
    public interface IConnectionFactory
    {
        public DbConnection GetConnection();

        public DbTransaction GetTransaction();
    }
}
