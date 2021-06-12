using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Orleans.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TestWebSilo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseOrleans((contex, builder) => 
                {
                    if (contex.HostingEnvironment.IsDevelopment())
                    {
                        builder.UseLocalhostClustering();
                        builder.AddMemoryGrainStorage("definitions");
                    }
                    else
                    {
                        // In Kubernetes, we use environment variables and the pod manifest
                        //builder.UseKubernetesHosting();

                        //// Use Redis for clustering & persistence
                        //var redisConnectionString = $"{Environment.GetEnvironmentVariable("REDIS")}:6379";
                        //builder.UseRedisClustering(options => options.ConnectionString = redisConnectionString);
                        //builder.AddRedisGrainStorage("definitions", options => options.ConnectionString = redisConnectionString);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services => 
                    {
                        services.AddControllers();
                        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
                    });
                    //webBuilder.UseStartup<Startup>();

                    webBuilder.Configure((context, app) =>
                    {
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.UseDefaultFiles();
                        app.UseStaticFiles();
                        app.UseRouting();
                        app.UseAuthorization();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                });
    }
}
