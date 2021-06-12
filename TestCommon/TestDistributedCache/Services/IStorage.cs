using System;
using System.Threading;
using System.Threading.Tasks;
using TestDistributedCache.Models;

namespace TestDistributedCache.Services
{
    public interface IStorage
    {
        Task AddCandleAsync(CandlesModel candles, CancellationToken cancellationToken = default);
        public Task<CandleModel> GetCandleAsync(string pairName, int frame, int minutesBefore);
        Task AddIndicatorsAsync(string pairName, string indicatorName, int frame, DateTime dateTime, decimal value, CancellationToken cancellationToken);
    }
}