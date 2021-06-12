using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace TestLogging
{
    internal class SomeHostedService : IHostedService
    {
        private readonly ILogger<SomeHostedService> logger;

        public SomeHostedService(ILogger<SomeHostedService> logger)
        {
            this.logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogError("adcnsdjcnskjdcn");
            //throw new System.NotImplementedException();
            await Task.Delay(4000);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            //throw new System.NotImplementedException();
        }
    }
}