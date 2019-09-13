using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace FortuneTellerWinService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void ServiceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
        }

        private void ServiceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
