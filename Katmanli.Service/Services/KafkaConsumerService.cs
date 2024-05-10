using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Service.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        //private readonly IRedisServer _redisServer;
        private readonly string _kafkaTopic;

        public KafkaConsumerService(IRedisServer redisServer)
        {
            //_redisServer = redisServer;
            _kafkaTopic = "192.168.20.104.dbo.UploadImages"; // Kafka'dan gelen mesajları dinlemek istediğiniz konu
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // _redisServer.SubscribeToKafkaTopic(_kafkaTopic);
                //SubscribeToKafkaTopic(_kafkaTopic);
                await Task.Delay(1000, stoppingToken); // Belirli bir süre bekleyerek döngüyü tekrar etmesini sağlar
            }
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
                        var message = consumer.Consume(); // Kafka'dan mesaj al
                        var jsonMessage = JObject.Parse(message.Message.Value);

                        // Access the payload
                        var payload = jsonMessage["payload"];
                        if (payload != null)
                        {
                            var after = payload["after"];
                            if (after != null)
                            {

                                var fileId = after["FileKey"].Value<string>();
                                var filePath = after["FilePath"].Value<string>();
                                // var fileName = after["FileOriginalName"].Value<string>();

                                Console.WriteLine($"FileKey: {fileId},  FilePath: {filePath}");
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
    }
}
