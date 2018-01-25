using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace windows_service
{
    [RunInstaller(runInstaller: true)]
    public class TrackerServiceInstaller : Installer
    {
        public TrackerServiceInstaller()
        {
            Installers.Add(new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                ServiceName = "Short name of service for Windows Service Control Manager goes here",
                DisplayName = "Display name of service for Windows Service Control Manager UI goes here",
                Description = "Service description for Windows Service Control Manager UI goes here"
            });

            Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            });
        }
    }
}