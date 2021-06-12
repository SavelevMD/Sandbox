using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TestGrainInterfaces;

namespace TestGrains
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private int counter;
        private readonly ILogger logger;

        public HelloGrain(ILogger<HelloGrain> logger)
        {
            this.logger = logger;
        }

        Task<string> IHello.SayHello(string greeting)
        {
            counter++;
            logger.LogInformation($"\n SayHello message received: greeting = '{greeting}'");
            return Task.FromResult($"\n Client said: '{greeting}', so HelloGrain says: Hello! Counter:{counter}");
        }
    }
}
