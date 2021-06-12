using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace TestBenchmarkDistributedCache
{
    class Program
    {
        public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
            .Run(args, DefaultConfig.Instance.AddJob(Job.Default.WithGcServer(true))/*, new DebugInProcessConfig()*/);
    }
}
