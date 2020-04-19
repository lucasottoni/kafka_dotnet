using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using Kafka.Client.Extensions;

namespace KafkaWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddKafka(options =>
                    {
                        options.ProducerConfig = new ProducerConfig
                        {
                            BootstrapServers = "localhost:9092",
                            MessageTimeoutMs = 6000
                        };
                        options.ConsumerConfig = new ConsumerConfig
                        {
                            GroupId = "compose-connect-group",
                            BootstrapServers = "localhost:9092",
                            AutoOffsetReset = AutoOffsetReset.Earliest,
                            EnableAutoCommit = false
                        };
                    });

                    services.AddHostedService<Worker>();
                });
    }
}
