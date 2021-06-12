using BenchmarkDotNet.Attributes;
using Newtonsoft.Json.Linq;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TestBenchmarkDistributedCache.Generator;

namespace TestBenchmarkDistributedCache.JsonCompetition
{
    public class NewtonVSMS
    {
        private IEnumerable<string> _candles;
        private static readonly string[] _fields = new string[] { "Open", "Close", "High", "Low" };

        [GlobalSetup]
        public void Setup()
        {
            _candles = CandleGenerator.GetCandlesToString(1000);
        }

        [Benchmark]
        public void NewtonsoftParser()
        {
            var resultList = new List<decimal>();
            foreach (var c in _candles)
            {
                var candle = JObject.Parse(c);
                foreach (var f in _fields)
                {
                    resultList.Add((decimal)candle[f]);
                }
            }
        }

        [Benchmark]
        public void MSParser()
        {
            var resultList = new List<decimal>();
            foreach (var c in _candles)
            {
                var message = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(c));
                foreach (var f in _fields)
                {
                    resultList.Add(GetPropertyValue(message, f));
                }
            }
        }

        private decimal GetPropertyValue(in ReadOnlySequence<byte> message, string propertyName)
        {
            try
            {
                var reader = new Utf8JsonReader(message, isFinalBlock: true, _readerState);
                if (reader.Read())
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName && reader.ValueTextEquals(propertyName))
                        {
                            reader.Read();
                            return reader.GetDecimal();
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private static readonly JsonReaderState _readerState = new JsonReaderState(new JsonReaderOptions
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip
        });
    }
}
