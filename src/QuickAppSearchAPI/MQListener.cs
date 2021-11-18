using CommonData.Models;
using CommonData.RabbitQueue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuickAppSearchAPI
{
    public class MQListener : BackgroundService
    {
        private readonly ILogger<MQListener> _logger;
        private readonly IBus _busControl;
        private readonly ICosmosDbService _cosmos;
        public MQListener(ILogger<MQListener> logger, ICosmosDbService cosmos)
        {
            _logger = logger;
            _busControl = RabbitHutch.CreateBus("myrabbitmq.eastus.azurecontainer.io");
            _cosmos = cosmos;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _busControl.ReceiveAsync<IEnumerable<ProductRequest>>(Queue.Processing, x =>
            {
                Task.Run(() => { DidJob(x); }, stoppingToken);
            });
        }

        private void DidJob(IEnumerable<ProductRequest> products)
        {
            foreach (var prd in products)
            {
                _logger.LogInformation($"Queue Message: {prd.Message}");
                Item logItem = new Item();
                logItem.Id = Guid.NewGuid().ToString();
                logItem.Name = $"Queue Message: {prd.Message}";
                logItem.Time = DateTime.Now;
                _cosmos.AddItemAsync(logItem);
            }
        }
    }
}
