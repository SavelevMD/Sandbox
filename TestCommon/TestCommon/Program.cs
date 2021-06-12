using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestCommon
{
    class Program
    {
       
        static async Task Main(string[] args)
        {
            var p = await Policy.Handle<Exception>()
                .RetryAsync(2, (ex, count) =>
                {
                    Console.WriteLine($"attempt{count}");
                })
                .ExecuteAndCaptureAsync(cancellationToken => NewMethod(cancellationToken), CancellationToken.None);
            Console.WriteLine($"{p.Outcome}");
            throw p.FinalException;
            //Console.ReadLine();
        }

        private static async Task NewMethod(CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            Console.WriteLine($"executon");
            throw new Exception("afafafaf");
        }
    }
}
