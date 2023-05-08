using Microsoft.Extensions.DependencyInjection;
using Services.Services;

namespace WebFramework.Configurations
{
    public static class ServiceCollectionExtensions
    {

        public static void ConfigJWTAuthentication(this IServiceCollection services)
        {
            services.AddAuthenticationCore(options =>
            {
                options.DefaultAuthenticateScheme=JwtBearerDefaults
            })
        }
    }
}
