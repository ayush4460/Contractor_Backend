using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Contractor_Backend.Persistence.DbContext; 


namespace Contractor_Backend.API.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("Database");
            return services;
        }
    }

}
