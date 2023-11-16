using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace AwesomeSocialMedia.Users.Infrastructure.EventBus
{
    public class RabbitMqService : IEventBus
    {
        private readonly IModel _channel;
        private const string Exchange = "user";

        public RabbitMqService()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = connectionFactory.CreateConnection("users.publisher");

            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(Exchange, "direct", true, false);
        }

        public async void Publish<T>(T @event) 
        {
            var routingKey = @event.GetType().Name.ToDashCase();

            Console.WriteLine(routingKey);

            var json = JsonConvert.SerializeObject(@event);
            var bytes = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(Exchange, routingKey, null, bytes);
        }
    }
}