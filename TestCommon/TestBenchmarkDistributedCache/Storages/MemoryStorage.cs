using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TestDistributedCache.Models;

namespace TestBenchmarkDistributedCache.Storages
{
    public class MemoryStorage
    {
        private ConcurrentDictionary<string, Dictionary<DateTimeOffset, CandleModel>> _collector = new ConcurrentDictionary<string, Dictionary<DateTimeOffset, CandleModel>>();
        private readonly object _syncObject = new object();

        public void AddCandle(CandlesModel candles)
        {
            _collector.TryAdd($"{candles.CurrencyName}:{candles.TimeFrame}", new Dictionary<DateTimeOffset, CandleModel>());
            lock (_syncObject)
            {
                candles.CandleCollection.Select(r => _collector[$"{candles.CurrencyName}:{candles.TimeFrame}"][r.ReceiptTime] = r).ToList();
            }
        }

        public CandleModel GetCandle(string pairName, int frame, int minutesBefore)
        {
            lock (_syncObject)
            {
                if (_collector.TryGetValue($"{pairName}:{frame}", out var candles))
                {
                    var dt = DateTime.Now.AddMinutes(-minutesBefore);
                    dt = dt.AddSeconds(-dt.Second);
                    return candles.TryGetValue(dt, out var candle) ? candle : null;
                }
                return null;
            }
        }
    }
}
