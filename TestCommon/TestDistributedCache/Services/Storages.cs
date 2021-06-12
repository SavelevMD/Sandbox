using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestDistributedCache.Models;

namespace TestDistributedCache.Services
{
    public class Storages : IStorage
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<Storages> _logger;

        public Storages(IDistributedCache distributedCache, ILogger<Storages> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task AddCandleAsync(CandlesModel candles, CancellationToken cancellationToken = default)
        {
            try
            {
                var tasksCollection = candles.CandleCollection.Select(r => _distributedCache.SetStringAsync($"candles:{candles.CurrencyName}:{candles.TimeFrame}:{r.ReceiptTime}", JsonConvert.SerializeObject(r)));
                await Task.WhenAll(tasksCollection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "В процессе добавления свечи произошла ошибка");
            }
        }

        public async Task<CandleModel> GetCandleAsync(string pairName, int frame, int minutesBefore)
        {
            try
            {
                var dt = DateTime.Now.AddMinutes(-minutesBefore);
                dt = dt.AddSeconds(-dt.Second);
                var candle = await _distributedCache.GetStringAsync($"candles:{pairName}:{frame}:{dt}");
                return JsonConvert.DeserializeObject<CandleModel>(candle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ну не получили свечи{ex.Message}");
                throw;
            }
        }

        public Task AddIndicatorsAsync(string pairName, string indicatorName, int frame, DateTime dateTime, decimal value, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
