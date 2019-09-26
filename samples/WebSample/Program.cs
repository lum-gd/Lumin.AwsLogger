using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace WebSample
{
    public class Program
    {
        static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Env}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static int Main(string[] args)
        {
            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext();
            if (Env == "Development")
            {
                loggerConfig = loggerConfig.WriteTo.Debug()
                    .WriteTo.Console(
                    // {Properties:j} added:
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} " +
                                    "{Properties:j}{NewLine}{Exception}");
            }
            Log.Logger = loggerConfig.CreateLogger();
            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(Configuration)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Debug()
            //    .WriteTo.Console(
            //        // {Properties:j} added:
            //        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} " +
            //                        "{Properties:j}{NewLine}{Exception}")
            //    .CreateLogger();

            try
            {
                Log.Information("Getting the motors running...");

                CreateWebHostBuilder(args).Build().Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(Configuration)
                .UseSerilog();
    }
}
