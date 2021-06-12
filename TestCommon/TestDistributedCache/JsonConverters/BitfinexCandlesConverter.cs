using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TestDistributedCache.Models;

namespace TestDistributedCache.JsonConverters
{
    public class BitfinexCandlesConverter : JsonConverter
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var data = JArray.Load(reader);
            var result = new CandlesModel();

            if (data.Count == 2)
            {
                result.ChannelId = (int)data[0];
                result.CandleCollection = new List<CandleModel>();
                if (((JArray)data[1]).Count > 6)
                {
                    foreach (var i in data[1])
                    {
                        result.CandleCollection.Add(CreateCandle((JArray)i));
                    }
                }
                else
                {
                    result.CandleCollection.Add(CreateCandle((JArray)data[1]));
                }
            }
            return result;
        }

        private CandleModel CreateCandle(JArray childJArray)
        {
            return new CandleModel()
            {
                ReceiptTime = DateTime.UnixEpoch.AddMilliseconds((long)childJArray[0]),
                Open = (decimal)childJArray[1],
                Close = (decimal)childJArray[2],
                High = (decimal)childJArray[3],
                Low = (decimal)childJArray[4],
                Volume = (decimal)childJArray[5]
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
