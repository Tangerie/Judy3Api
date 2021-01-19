using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Judy.Models;
using Judy.Modules;
using System.IO;
using Newtonsoft.Json;

//https://trello.com/b/HKAIorbZ/judy-3

namespace Judy
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Storage.CreateInstance("judy.db");
            Console.WriteLine($"Database Intialized (SQLite {Storage.Instance.GetVersion()}) at {Storage.Instance.DatabaseLocation}");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
