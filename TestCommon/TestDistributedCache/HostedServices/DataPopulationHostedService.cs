using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestDistributedCache.Models;
using TestDistributedCache.Services;

namespace TestDistributedCache.HostedServices
{
    public class DataPopulationHostedService : IHostedService
    {
        private readonly IStorage _storage;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<DataPopulationHostedService> _logger;
        private readonly ISubscriber _subscriber;
        private RedisValue _value;

        public DataPopulationHostedService(IStorage storage, IConnectionMultiplexer connectionMultiplexer, ILogger<DataPopulationHostedService> logger)
        {
            _storage = storage;
            _connectionMultiplexer = connectionMultiplexer;
            _subscriber = _connectionMultiplexer.GetSubscriber();
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _subscriber.SubscribeAsync("candles", CandleHandler);
        }

        private async void CandleHandler(RedisChannel channel, RedisValue value)
        {
            _value = value;
            await StartBenchmark();
        }

        [Benchmark]
        public async Task StartBenchmark()
        {
            try
            {
                var candles = JsonConvert.DeserializeObject<CandlesModel>(_value);
                await _storage.AddCandleAsync(candles);

                await _storage.GetCandleAsync("ETHUSD", 1, 20);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При обработке свечи по подписке произошла ошибка");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
