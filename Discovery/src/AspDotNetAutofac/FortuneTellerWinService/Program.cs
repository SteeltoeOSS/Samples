using System;
using System.ServiceProcess;

namespace FortuneTellerWinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Console.WriteLine("Starting Program.Main");
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FortuneService()
            };
            Console.WriteLine("ServiceBase.Run");
            ServiceBase.Run(ServicesToRun);
        }
    }
}
