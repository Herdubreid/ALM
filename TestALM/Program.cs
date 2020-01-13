using Blazor.Extensions.Storage.Interfaces;
using BlazorState;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Reflection;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Config file
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();

                // Initialise the Logger
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                        .AddConfiguration(config.GetSection("Logging"))
                        .AddConsole();
                });
                ILogger logger = loggerFactory.CreateLogger<Program>();

                // Build the Service Collection
                var services = new ServiceCollection()
                    .AddSingleton(logger)
                    .AddSingleton(config)
                    .AddSingleton<HttpClient>()
                    .AddSingleton<Celin.AIS.Server, E1>()
                    .AddScoped<ILocalStorage, FileStorage>()
                    .AddBlazorState(
                        options => options.Assemblies = new Assembly[]
                        {
                            typeof(Celin.ALMState).GetTypeInfo().Assembly
                        }
                    )
                    .AddScoped<Celin.ALMState>()
                    .BuildServiceProvider();

                // Build the Command Line Parser
                var app = new CommandLineApplication<Cmd>();
                app.Conventions
                    .UseDefaultConventions()
                    .UseConstructorInjection(services);
                app.OnExecute(() => app.ShowHelp());

                app.Execute(args);
            }
            catch (Celin.AIS.HttpWebException e)
            {
                Console.WriteLine("ERROR: {0}", e.ErrorResponse.message);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
        }
    }
}
