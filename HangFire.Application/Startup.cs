using HangFire.Application.Contracts;
using HangFire.Application.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace HangFire.Application
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices ( this IServiceCollection services )
        {
            services.AddScoped<IUserService , UserService>();
            services.AddTransient<IMailService , MailService>();

            return services;
        }
    }
}
