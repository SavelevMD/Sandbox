using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading;
using System.Threading.Tasks;
using TestDistributedCache.Services;

namespace TestDistributedCache.HostedServices
{
    public class CandlePublisher : IHostedService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly CandleGenerator _candleGenerator;
        private readonly ISubscriber _publisher;

        public CandlePublisher(IConnectionMultiplexer connectionMultiplexer, CandleGenerator candleGenerator)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _candleGenerator = candleGenerator;
            _publisher = _connectionMultiplexer.GetSubscriber();
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var candles = _candleGenerator.GetCandles(9999);
            foreach (var c in candles)
            { 
                await _publisher.PublishAsync("candles", JsonConvert.SerializeObject(c));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
