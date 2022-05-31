using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;

namespace HWA_GARDEN.KeyGen
{
    internal class KeyGen : BackgroundService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public KeyGen(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine();
                Console.Write("Please, provide the string for decrypting:");
                var input = Console.ReadLine();

                IDataProtector? dataProtector = _dataProtectionProvider.CreateProtector("HWA.GARDEN.Security.SecurityContext");
                var output = dataProtector.Protect(input);
                Console.Write("Encrypted string:");
                Console.WriteLine(output);

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
