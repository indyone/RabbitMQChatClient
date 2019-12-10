using RabbitMQ.Client;
using Xunit;
using static RabbitMQChatClient.Tests.Helper;

namespace RabbitMQChatClient.Tests
{
    public class MessageTests
    {
        private readonly IConnection _connection;

        public MessageTests()
        {
            _connection = GetConnection();
        }

        [Fact]
        public void BasicPublish_WithValidMemberJoinBody_ShouldDisplayProperly()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "join",
                nickname = "test",
                timestamp = CurrentTimestamp
            };

            channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
        }

        [Fact]
        public void BasicPublish_WithValidMemberLeaveBody_ShouldDisplayProperly()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "leave",
                nickname = "test",
                timestamp = CurrentTimestamp
            };

            channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
        }

        [Fact]
        public void BasicPublish_WithValidPublishMessageBody_ShouldDisplayProperly()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "publish",
                nickname = "test",
                timestamp = CurrentTimestamp,
                message = "This is a publish message and should display properly"
            };

            channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
        }

        [Fact]
        public void BasicPublish_WithRoutingKey_ShouldDisplayProperly()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "publish",
                nickname = "test",
                timestamp = CurrentTimestamp,
                message = "This is a publish message with a routing key and should display properly"
            };

            channel.BasicPublish(ExchangeName, "test", body: body.ToJsonBytes());
        }

        [Fact]
        public void BasicPublish_WithMessageKeyInMemberJoin_ShouldNotDisplayMessageValue()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "join",
                nickname = "test",
                timestamp = CurrentTimestamp,
                message = "This is a join message and you should not display this value"
            };

            channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
        }

        [Fact]
        public void BasicPublish_WithExtraKeyInPublishMessage_ShouldIgnoreKey()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "publish",
                nickname = "test",
                timestamp = CurrentTimestamp,
                message = "This is a publish message with an extra key and should display properly",
                extra = true
            };

            channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
        }

        [Fact]
        public void BasicPublish_WithInvalidTimestampValue_ShouldBeIgnored()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "publish",
                nickname = "test",
                timestamp = -1337,
                message = "This is a publish message with an invalid timestamp value and should be ignored"
            };

            channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
        }

        [Fact]
        public void BasicPublish_WithInvalidNicknameValue_ShouldBeIgnored()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "publish",
                nickname = new { value1 = "test" },
                timestamp = CurrentTimestamp,
                message = "This is a publish message with an invalid nickname value and should be ignored"
            };

            channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
        }

        [Fact]
        public void BasicPublish_WithInvalidMessageValue_ShouldBeIgnored()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "publish",
                nickname = "test",
                timestamp = CurrentTimestamp,
                message = new { value2 = "This is a publish message with an invalid message value and should be ignored" }
            };

            channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
        }

        [Fact]
        public void BasicPublish_WithInvalidTypeValue_ShouldBeIgnored()
        {
            var channel = _connection.CreateModel();

            var body = new
            {
                type = "invalid",
                nickname = "test",
                timestamp = CurrentTimestamp,
                message = "This is an invalid message and should not be displayed at all"
            };

            channel.BasicPublish(ExchangeName, "", body: body.ToJsonBytes());
        }
    }
}
