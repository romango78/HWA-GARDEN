namespace HWA.GARDEN.Common.Security
{
    public class SecurityContext : ISecurityContext
    {
        public SecurityContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; private set; }
    }
}
