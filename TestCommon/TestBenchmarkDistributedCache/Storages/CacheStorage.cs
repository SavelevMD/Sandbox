using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestDistributedCache.Models;

namespace TestBenchmarkDistributedCache.Storages
{
    static class CacheStorage
    {
        public static async Task AddCandleAsync(CandlesModel candles, IDistributedCache cache, CancellationToken cancellationToken = default)
        {
            try
            {
                var tasksCollection = candles.CandleCollection.Select(r => cache.SetStringAsync($"candles:{candles.CurrencyName}:{candles.TimeFrame}:{r.ReceiptTime}", JsonConvert.SerializeObject(r)));
                await Task.WhenAll(tasksCollection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"В процессе добавления свечи произошла ошибка{Environment.NewLine}{ex.Message}");
            }
        }

        public static async Task<CandleModel> GetCandleAsync(IDistributedCache cache, string pairName, int frame, int minutesBefore)
        {
            var dt = DateTime.Now.AddMinutes(-minutesBefore);
            try
            {
                dt = dt.AddSeconds(-dt.Second);

                var candle = await cache.GetStringAsync($"candles:{pairName}:{frame}:{dt}");
                return JsonConvert.DeserializeObject<CandleModel>(candle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ну не получили свечи{ex.Message}{Environment.NewLine}candles:{pairName}:{frame}:{dt}");
                return null;
            }
        }


    }
}
