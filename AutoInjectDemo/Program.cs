using Microsoft.Extensions.DependencyInjection;

namespace AutoInjectDemo
{
    // Four inject type can scan DIHelper.cs => ConfigureServices
    internal class Program
    {
        static void Main(string[] args)
        {
            var scopeFactory = DIHelper.ServiceProvider.GetRequiredService<IServiceScopeFactory>();

            Action<IServiceProvider> testActionF = (services) =>
            {
                var singletonTest = services.GetRequiredService<ISingletonTest>();
                var scopedTest = services.GetRequiredService<IScopedTest>();
                var transientTest = services.GetRequiredService<ITransientTest>();
            };
            Action<IServiceProvider> testActionAttributeF = (services) =>
            {
                var singletonTest = services.GetRequiredService<ISingletonAttributeTest>();
                var scopedTest = services.GetRequiredService<IScopedAttributeTest>();
                var transientTest = services.GetRequiredService<ITransientAttributeTest>();
            };

            Action testAction = () => testActionF(DIHelper.ServiceProvider);


            Action testScopeAction = () =>
            {
                using (var scope = scopeFactory.CreateScope()) testActionF(scope.ServiceProvider);
            };

            Action testAttributeAction = () => testActionAttributeF(DIHelper.ServiceProvider);

            Action testScopeAttributeAction = () =>
            {
                using (var scope = scopeFactory.CreateScope()) testActionAttributeF(scope.ServiceProvider);
            };

            DO_TEST(1, () =>
            {
                for (int i = 1; i < 6; i++)
                    testAction();
            });

            DO_TEST(2, () =>
            {
                for (int i = 1; i < 6; i++)
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    PRINTF($"scope{i}");
                    Console.ForegroundColor = color;
                    testScopeAction();
                }
            });

            DO_TEST(3, () =>
            {
                for (int i = 1; i < 6; i++)
                    testAttributeAction();
            });

            DO_TEST(4, () =>
            {
                for (int i = 1; i < 6; i++)
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    PRINTF($"scope{i}");
                    Console.ForegroundColor = color;
                    testScopeAttributeAction();
                }
            });


            PRESS_KEY_QUIT();
        }


        static Action<int, Action> DO_TEST = (id, foo) =>
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            PRINTF($"==================== start {id,-3} ====================");
            Console.ForegroundColor = ConsoleColor.Green;
            foo?.Invoke();
            Console.ForegroundColor = ConsoleColor.Blue;
            PRINTF($"====================  end  {id,-3} ====================\n");
            Console.ForegroundColor = ConsoleColor.White;
        };

        static Action PRESS_KEY_QUIT = () =>
        {
            var key = Console.ReadKey();

            while (key.Key != ConsoleKey.Escape)
            {
                key = Console.ReadKey();
            }
        };

        static Action<string> PRINTF = (s) =>
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {s}");
    }
}