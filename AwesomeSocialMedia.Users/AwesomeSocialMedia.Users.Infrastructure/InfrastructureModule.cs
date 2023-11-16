using AwesomeSocialMedia.Users.Core.Repositories;
using AwesomeSocialMedia.Users.Infrastructure.EventBus;
using AwesomeSocialMedia.Users.Infrastructure.Persistence;
using AwesomeSocialMedia.Users.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Consul;

namespace AwesomeSocialMedia.Users.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AwesomeSocialMediaCs");

            services
                .AddDb(connectionString)
                .AddRepositories()
                .AddEventBus()
                .AddConsul(configuration);

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder) 
        {
            builder
                .UseConsul();

            return builder;
        }

        private static IApplicationBuilder UseConsul(this IApplicationBuilder app) 
        {
            var consultClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("Consul");

            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            var serviceId = Guid.NewGuid();

            var registration = new AgentServiceRegistration
            {
                ID = $"users={serviceId}",
                Name = "Users",
                Address = "localhost",
                Port = 5131
            };

            logger.LogInformation("Registrando o Consul.");

            consultClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consultClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            lifetime.ApplicationStopped.Register(() => 
            {
                logger.LogInformation("Desregistrando do Consul.");
                consultClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            });

            return app;
        }

        private static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => 
                new ConsulClient(config => 
                {
                    var adress = configuration.GetValue<string>("Consul:Host");

                    config.Address = new Uri(adress);
                }));

            return services;
        }

        private static IServiceCollection AddDb(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddScoped<IEventBus, RabbitMqService>();

            return services;
        }
    }
}