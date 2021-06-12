using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Threading.Tasks;

namespace TestCommon
{
    public class RetryOperationPattern
    {
        private readonly ILogger<RetryOperationPattern> _logger;

        public RetryOperationPattern(ILogger<RetryOperationPattern> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteWhileNotSucccessAsync<TValue, TResult>(Func<TValue, Task<TResult>> func, TValue value)
        {
            await Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync(t => TimeSpan.FromSeconds(60/t),
                                         (e, retryCount, ts) =>
                                         {
                                             Console.WriteLine($"В процессе выполнения метода, на  {retryCount} попытке, произошла ошибка");
                                             _logger.LogError(e, $"В процессе выполнения метода, на  {retryCount} попытке, произошла ошибка");
                                         })
                .ExecuteAsync(async () => await func(value));
        }
    }
}
