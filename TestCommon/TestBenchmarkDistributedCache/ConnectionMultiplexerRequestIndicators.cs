using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestBenchmarkDistributedCache.Models;

namespace TestBenchmarkDistributedCache
{
    public class ConnectionMultiplexerRequestIndicators
    {
        public static string DebugConnection = "localhost:32768,abortConnect=false";
        private ConnectionMultiplexer _connection;

        [GlobalSetup]
        public void Setup()
        {
            _connection = ConnectionMultiplexer.Connect(DebugConnection);
        }

        [Benchmark]
        public async Task RequestIndicators()
        {
            var tasks = await _connection.GetDatabase()
                                                .HashGetAllAsync("tETHUSD");
            var indicators = new HashSet<string>();
            foreach (var task in tasks)
            {
                var t = JsonConvert.DeserializeObject<TaskWrapper>(task.Value);
                if (t.TimeFrame.Equals("1m", StringComparison.OrdinalIgnoreCase))
                {
                    t?.Indicators?.ForEach(r => indicators.Add(r));
                }
            }
        }

        [Benchmark]
        public async Task RequestCandles()
        {
            await Task.Yield();
        }
    }
}
