using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IOC
{
    public static class ServiceTool
    {
        
        public static IServiceProvider ServiceProvider { get; private set; }
        // burada .net in kendi IOC özelliğini kullanıyor autofaci  değil.
        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
