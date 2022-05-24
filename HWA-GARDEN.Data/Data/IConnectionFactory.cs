using System.Data;

namespace HWA.GARDEN.Common.Data
{
    public interface IConnectionFactory
    {
        public IDbConnection GetConnection();

        public IDbTransaction GetTransaction();
    }
}
