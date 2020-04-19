using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;

namespace KafkaConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "consumer-produto",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe("produtos");
                var cts = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var message = c.Consume(cts.Token);

                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        _logger.LogInformation($"Mensagem: {message.Message.Value} recebida de {message.TopicPartitionOffset}");
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }

            return Task.CompletedTask;
        }
    }
}
