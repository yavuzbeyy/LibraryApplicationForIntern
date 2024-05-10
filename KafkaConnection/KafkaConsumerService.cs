namespace KafkaConnection
{
    using Confluent.Kafka;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json.Linq;
    using StackExchange.Redis;

    namespace KafkaListener
    {
        public class KafkaConsumerService : BackgroundService
        {
            private readonly string _kafkaTopic;

            private readonly IDatabase _redisDatabase;
            string redisConnectionString = "localhost:6379,abortConnect=false";
            private readonly ConnectionMultiplexer _redisConnection;

            public KafkaConsumerService(ConnectionMultiplexer redisConnection)
            {
                _kafkaTopic = "192.168.20.104.dbo.UploadImages"; // Kafka'dan gelen mesajları dinlemek istediğiniz konu

                _redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
                _redisDatabase = _redisConnection.GetDatabase();
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {

            }

            public void StartListening()
            {
                SubscribeToKafkaTopic(_kafkaTopic);
            }

            public void SubscribeToKafkaTopic(string topic)
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = "localhost:59092", // Kafka sunucusunun adresi ve portu
                    GroupId = "redis-sync-consumer-group5",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(topic);

                    while (true)
                    {
                        try
                        {
                            var message = consumer.Consume();
                            if (message.Message != null && message.Message.Value != null)
                            {
                                var jsonMessage = JObject.Parse(message.Message.Value);
                                var payload = jsonMessage["payload"];

                                if (payload != null)
                                {
                                    var after = payload["after"];

                                    if (after != null && after.Type == JTokenType.Object)
                                    {
                                        var fileId = after["FileKey"]?.Value<string>();
                                        var filePath = after["FilePath"]?.Value<string>();

                                        if (!string.IsNullOrEmpty(fileId) && !string.IsNullOrEmpty(filePath))
                                        {
                                            StoreFilePath(fileId, filePath);
                                            Console.WriteLine($"FileKey: {fileId},  FilePath: {filePath}");
                                        }
                                    }
                                }
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occurred: {e.Error.Reason}");
                        }

                    }
                }

            }

            public void StoreFilePath(string filekey, string filePath)
            {
                _redisDatabase.HashSet("filepaths", filekey, filePath);
            }

            public string GetFilePath(string filekey)
            {
                return _redisDatabase.HashGet("filepaths", filekey);
            }
        }
    }

}
