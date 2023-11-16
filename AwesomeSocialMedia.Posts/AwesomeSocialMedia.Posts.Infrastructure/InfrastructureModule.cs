using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AwesomeSocialMedia.Posts.Infrastructure.Persistence.Repositories;
using AwesomeSocialMedia.Posts.Core.Repositories;
using AwesomeSocialMedia.Posts.Infrastructure.EventBus;
using Microsoft.EntityFrameworkCore;
using AwesomeSocialMedia.Posts.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Consul;

using AwesomeSocialMedia.Posts.Infrastructure.Integration.Services;

namespace AwesomeSocialMedia.Posts.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            var connectionString = configuration.GetConnectionString("");

            services
                // .AddDb(connectionString)
                .AddRepositories()
                .AddEventBus()
                .AddConsul(configuration)
                .AddIntegrationService();

            services.AddHttpClient<IUserIntegrationService, UserIntegrationService>();

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
                ID = $"posts={serviceId}",
                Name = "Posts",
                Address = "localhost",
                Port = 5080
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

        private static IServiceCollection AddDb(this IServiceCollection services, string connectionString) 
        {
            services.AddDbContext<PostsDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IPostRepository, PostRepository>();

            return services;
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddScoped<IEventBus, RabbitMqService>();

            return services;
        }

        private static IServiceCollection AddIntegrationService(this IServiceCollection services) 
        {
            services.AddScoped<IUserIntegrationService, UserIntegrationService>();

            return services;
        }

    }
}
