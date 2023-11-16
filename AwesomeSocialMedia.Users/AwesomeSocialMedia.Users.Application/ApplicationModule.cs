using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeSocialMedia.Users.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediator();

            return services;
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationModule));

            return services;
        }
    }
}
