using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RabbitMQChatClient.Tests
{
    /// <summary>
    ///  Contains helper methods for the tests.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Returns a new RabbitMQ connection and declares the exchange.
        /// </summary>
        public static IConnection GetConnection()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionFactory = new ConnectionFactory();

            configuration.Bind("RabbitMQ", connectionFactory);

            var connection = connectionFactory.CreateConnection("RabbitMQChatClient.Tests");

            // make sure the Exchange exists
            //connection.CreateModel().ExchangeDeclare(ExchangeName, ExchangeType.Fanout, true, false, null);

            return connection;
        }

        /// <summary>
        /// Returns the current timestamp.
        /// </summary>
        public static int CurrentTimestamp => (int)(DateTimeOffset.UtcNow - DateTimeOffset.UnixEpoch).TotalSeconds;

        /// <summary>
        /// Returns the exchange name.
        /// </summary>
        public static string ExchangeName => "chat_fnt";

        /// <summary>
        /// Serializes the <paramref name="value"/> to JSON and converts them to a byte array.
        /// </summary>
        public static byte[] ToJsonBytes(this object value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value);
        }
    }
}
