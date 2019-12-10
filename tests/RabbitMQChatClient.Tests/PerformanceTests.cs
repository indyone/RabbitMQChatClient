using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Xunit;
using static RabbitMQChatClient.Tests.Helper;

namespace RabbitMQChatClient.Tests
{
    public class PerformanceTests
    {
        private readonly IConnection _connection;

        public PerformanceTests()
        {
            _connection = GetConnection();
        }

        [Fact]
        public void BasicPublish_WithOneThousandConcurrentMessages_ShouldDisplayAllProperly()
        {
            var tasks = new List<Task>();

            var i = 0;
            while (i < 1000)
            {
                var id = i;

                var task = Task.Run(() =>
                {
                    var channel = _connection.CreateModel();

                    var body = new
                    {
                        type = "publish",
                        nickname = $"test_{id}",
                        timestamp = CurrentTimestamp,
                        message = $"This is the publish message #{id}"
                    };

                    channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
                });

                tasks.Add(task);

                i++;
            }

            Task.WhenAll(tasks);
        }

        [Fact]
        public void ExchangeDeclare_DeleteAndDeclareExchange_ShouldNotCrashTheApplication()
        {
            var channel = _connection.CreateModel();

            channel.ExchangeDelete(ExchangeName);

            Thread.Sleep(1000);

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout, true, false, null);
        }
    }
}
