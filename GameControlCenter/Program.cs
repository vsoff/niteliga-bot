using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameControlCenter.Unity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace GameControlCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            // Создаём экземпляр Unity контейнера и регистрируем все необходимые зависимости.
            IUnityContainer container = new UnityContainer();
            UnityConfigurationModule.Register(container);

            return WebHost.CreateDefaultBuilder(args)
                .UseUnityServiceProvider(container)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
