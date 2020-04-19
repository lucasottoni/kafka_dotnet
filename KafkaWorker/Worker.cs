using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Kafka.Client.Extensions;

namespace KafkaWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IKafkaClient<string, string> kafkaClient;

        public Worker(ILogger<Worker> logger, IKafkaClient<string, string> kafkaClient)
        {
            _logger = logger;
            this.kafkaClient = kafkaClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var result = kafkaClient.Consume("produtos");

                if (result != null)
                {
                    _logger.LogInformation($"Consumed message {result.Value}");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
