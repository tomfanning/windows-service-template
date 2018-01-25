using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace windows_service
{
    public delegate void LogDelegate(string format, params object[] args);

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(params string[] args)
        {
            return ServiceMain(Log, new ServiceImpl(Log), args);
        }

        static void Log(string format, params object[] args)
        {
            // you probably want to do more than this
            Console.WriteLine(format, args);
        }

        static int ServiceMain(LogDelegate logger, ServiceBase service, string[] args)
        {
            logger("Starting up");

            if (Environment.UserInteractive)
            {
                logger("UserInteractive");

                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "--install":
                        try
                        {
                            ManagedInstallerClass.InstallHelper(new string[] { "/LogToConsole", Assembly.GetEntryAssembly().Location });
                            return 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            Log(ex.ToString());
                            return -1;
                        }
                    case "--uninstall":
                        try
                        {
                            ManagedInstallerClass.InstallHelper(new string[] { "/u", "/LogToConsole", Assembly.GetEntryAssembly().Location });
                            return 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            Log(ex.ToString());
                            return -1;
                        }
                    default:
                        if (Debugger.IsAttached)
                        {
                            Debugger.Break();
                            var method = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance);
                            method.Invoke(service, new object[] { null });
                            Thread.CurrentThread.Join();
                            return 0;
                        }
                        else
                        {
                            Console.WriteLine("This is a service. Start it through Service Control Manager.");
                            return 0;
                        }
                }
            }
            else
            {
                logger("Non-interactive. Instantiating services.");

                var ServicesToRun = new ServiceBase[] { service };

                logger("Running service");
                ServiceBase.Run(ServicesToRun);

                logger("Returning");
                return 0;
            }
        }
    }
}
