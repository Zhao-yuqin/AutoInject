using Microsoft.Extensions.DependencyInjection;
using Qin.AutoInject;
using System.IO;
using System.Reflection;

namespace AutoInjectDemo
{
    public static class DIHelper
    {
        public static IServiceProvider ServiceProvider { get; set; }
        static DIHelper()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var defaultNamespace = "AutoInjectDemo";

            // 1: Just input your default namespace or null
            services.Inject(); // start slowly,but it can make you become lazier
            //services.Inject(defaultNameSpace);

            // 2: Single assembly input
            //services.Inject(Assembly.GetAssembly(typeof(Program)));

            // 3: You can add assembly manually load from assembly
            //var path = AppDomain.CurrentDomain.BaseDirectory;
            //var assemblies = Directory.GetFiles(path, "*.dll")
            //                          .Select(Assembly.LoadFrom)
            //                          .Where(d => d.FullName.Contains(defaultNamespace))
            //                          .ToList();
            //services.Inject(assemblies);

            // 4: Input '.dll' filenames
            //services.Inject(new List<string> { "AutoInjectDemo.dll" });
        }

    }
}
