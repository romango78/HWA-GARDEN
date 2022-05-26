using System.Data.Common;

namespace HWA.GARDEN.Utilities.Data
{
    public static class DataExtensions
    {
        public static DataReader<T> GetReader<T>(this DbDataReader reader)
        {
            return new DataReader<T>(reader);
        }
    }
}
