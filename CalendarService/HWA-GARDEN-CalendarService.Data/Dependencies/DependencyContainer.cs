using HWA.GARDEN.Common.Data;
using HWA.GARDEN.CalendarService.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

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

            builder.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.AddScoped<Func<IUnitOfWork>>(sp => () => { return sp.GetRequiredService<IUnitOfWork>(); });
        }
    }
}
