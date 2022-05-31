using HWA.GARDEN.Utilities.Validation;
using Microsoft.AspNetCore.DataProtection;

namespace HWA.GARDEN.Security
{
    public class SecurityContext : ISecurityContext
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly string _protectedConnectionString;
        private readonly string _dataProtectionPurpose;

        public SecurityContext(IDataProtectionProvider dataProtectionProvider, string protectedConnectionString, string dataProtectionPurpose)
        {
            Requires.NotNull(dataProtectionProvider, nameof(dataProtectionProvider));
            Requires.NotNull(protectedConnectionString, nameof(protectedConnectionString));

            _dataProtectionProvider = dataProtectionProvider;
            _protectedConnectionString = protectedConnectionString;
            _dataProtectionPurpose = dataProtectionPurpose ?? typeof(SecurityContext).FullName;
        }

        public string ConnectionString
        { 
            get
            {
                IDataProtector? dataProtector = _dataProtectionProvider.CreateProtector(_dataProtectionPurpose);
                return dataProtector.Unprotect(_protectedConnectionString);
            }
        }
    }
}
