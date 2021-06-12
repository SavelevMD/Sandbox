using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBenchmarkDistributedCache.Generator;
using TestBenchmarkDistributedCache.Storages;
using TestDistributedCache.Models;

namespace TestBenchmarkDistributedCache
{
    public class GetCandles
    {
        public static string DebugConnection = "localhost:32768,abortConnect=false";
        private IDistributedCache _cache;
        private MemoryStorage _memoryStorage;
        private IEnumerable<CandlesModel> _candles;

        [GlobalSetup]
        public async void Setup()
        {
            var services = new ServiceCollection();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost:32768";
            });

            services.AddSingleton<MemoryStorage>();

            var serviceProvider = services.BuildServiceProvider();
            _cache = serviceProvider.GetService<IDistributedCache>();
            _memoryStorage = serviceProvider.GetService<MemoryStorage>();

            _candles = CandleGenerator.GetCandles(9000);
            await Task.WhenAll(_candles.Select(r => CacheStorage.AddCandleAsync(r, _cache)));

            _candles.ToList().ForEach(r => _memoryStorage.AddCandle(r));
        }

        //[Params(20, 30, 40)]
        //public int Counts { get; set; }

        //[Benchmark]
        //public async Task GetCandlesAsync()
        //{
        //    var tasks = Enumerable.Range(10, Counts).Select(r => Storage.GetCandleAsync(_cache, "ETHUSD", 1, r));
        //    await Task.WhenAll(tasks);
        //}

        [Benchmark]
        public async Task GetCandlesCollectionAsync()
        {
            var tasks = Enumerable.Range(2, 2000).Select(r => CacheStorage.GetCandleAsync(_cache, "ETHUSD", 1, r));
            await Task.WhenAll(tasks);
        }

        [Benchmark]
        public void GetCandlesCollectionMemoryStorage()
        {
            var candles = Enumerable.Range(2, 2000).Select(r => _memoryStorage.GetCandle("ETHUSD", 1, r)).ToList();
        }
    }
}
