using System;
using System.Text;
using AwesomeSocialMedia.Newsfeed.API.Core.Entities;
using AwesomeSocialMedia.Newsfeed.API.Core.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AwesomeSocialMedia.Newsfeed.API.Consumers
{
    public class UserUpdatedConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;
        private const string Queue = "newsfeed.user-updated";
        private const string Exchange = "user";
        private const string RoutingKey = "user-updated";

        public UserUpdatedConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var connectionFactory = new ConnectionFactory() 
            {
                HostName = "localhost"
            };

            var connection = connectionFactory.CreateConnection("newsfeed.user-updated");

            _channel = connection.CreateModel();

            _channel.QueueDeclare(Queue, true, false, false, null);

            _channel.ExchangeDeclare(Exchange, "direct", true, false);

            _channel.QueueBind(Queue, Exchange, RoutingKey);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) => 
            {
                var contentArray = eventArgs.Body.ToArray();
                var json = Encoding.UTF8.GetString(contentArray);
                var @event = JsonConvert.DeserializeObject<UserUpdated>(json);

                Console.WriteLine(json);

                await UpdateUserNewsFeed(@event);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(Queue, false, consumer);

            return Task.CompletedTask;
        }

        public async Task UpdateUserNewsFeed(UserUpdated @event)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IUserNewsfeedRepository>();

                await repository.UpdateUserNewsFeed(@event.ToEntity());
            }
        }
    }

    public class UserUpdated 
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }

        public User ToEntity() => new User { Id = Id, DisplayName = DisplayName };
    }
}

