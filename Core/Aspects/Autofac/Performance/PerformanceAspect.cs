using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IOC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private int _interval;
        private Stopwatch _stopwatch;

        public PerformanceAspect(int interval)
        {
            // interval verilen süre
            _interval = interval;
            // yine bu bir aspect olduğu için katmanlar arası zincire dahil olamadığı için burada bu şekilde newleriz.
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }


        protected override void OnBefore(IInvocation invocation)
        {
            // öncesinde timer başlatılır.
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            { // süre daha yüksek çıkarsa beni console ile uyaracak.
                Debug.WriteLine($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
            }
            _stopwatch.Reset();
        }
    }
}
