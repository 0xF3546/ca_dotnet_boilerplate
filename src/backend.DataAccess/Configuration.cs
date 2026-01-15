using backend.Core.Auth;
using backend.DataAccess.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace backend.DataAccess
{
    public static class Configuration
    {
        public static void ConfigureDataAccess(this IServiceCollection services)
        {
            services.AddScoped<IExternalAuthService, ExternalAuthService>();
        }
    }
}
