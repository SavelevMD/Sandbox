using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using TestDistributedCache.HostedServices;
using TestDistributedCache.Services;

namespace TestDistributedCache
{
    public class Startup
    {


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedRedisCache(options => 
            {
                options.Configuration = "localhost:32768,abortConnect=false";
            });
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(ConfigurationOptions.Parse("localhost:32768,abortConnect=false")));
            services.AddSingleton<CandleGenerator>();
            services.AddHostedService<DataPopulationHostedService>();
            services.AddHostedService<CandlePublisher>();
            services.AddSingleton<IStorage, Storages>();
            services.AddLogging();

            //var summary = BenchmarkRunner.Run<DataPopulationHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
