using HWA.GARDEN.CalendarService.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using HWA.GARDEN.Data;

namespace HWA.GARDEN.CalendarService.Data.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder)
        {
            builder.AddScoped<Func<DbTransaction, ICalendarRepository>>(sp => (t) => 
            { 
                return new CalendarRepository(t);
            });

            builder.AddScoped<IConnectionFactory, ConnectionFactory>();

            builder.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.AddScoped<Func<IUnitOfWork>>(sp => () => { return sp.GetRequiredService<IUnitOfWork>(); });
        }
    }
}
