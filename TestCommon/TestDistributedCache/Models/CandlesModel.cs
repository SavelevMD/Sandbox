using System.Collections.Generic;
using System.Text.Json.Serialization;
using TestDistributedCache.JsonConverters;

namespace TestDistributedCache.Models
{
    [JsonConverter(typeof(BitfinexCandlesConverter))]
    public class CandlesModel
    {
        public string CurrencyName { get; set; }

        public int TimeFrame { get; set; }

        public int ChannelId { get; set; }

        public IList<CandleModel> CandleCollection { get; set; }
    }
}
