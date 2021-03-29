using Core.CrossCuttingConcern.Caching;
using Core.CrossCuttingConcern.Caching.Microsoft;
using Core.Utilities.IOC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule       // Ortak coreda olması gereken bağlılıklar. 
                                                // Bu bağlılılar katmanlar arasında zincirle bağlı değil. Bu mıdül ile araya sokuyoruz.
    {
        public void Load(IServiceCollection serviceCollection)
        {
            // memoryCache i injecktion ın çalışabilmesi için aşağıdaki satırı çalıştırman lazım. NetCore kendisi instance oluşturuyor.
            serviceCollection.AddMemoryCache();

            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();//birisi senden icachemanager istersen memorycachemanager ver.
            serviceCollection.AddSingleton<Stopwatch>();

        }
    }
}
