using FileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChatServer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // fixes the blurriness of the form - https://www.youtube.com/watch?v=-pmER189dWQ
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // start of code for creating our services 
            var services = new ServiceCollection();
            ConfigureServices(services);

            using ServiceProvider serviceProvider = services.BuildServiceProvider();

            ChatServerGUI gui = serviceProvider.GetRequiredService<ChatServerGUI>();

            Application.Run(gui);

        }

        // fixes the blurriness of the form - https://www.youtube.com/watch?v=-pmER189dWQ
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddDebug();
                configure.AddProvider(new CustomFileLogProvider());
                configure.AddEventLog();
                configure.SetMinimumLevel(LogLevel.Debug);
            });
            services.AddScoped<ChatServerGUI>();
        }

    }
}