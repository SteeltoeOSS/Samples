using Microsoft.Owin.Hosting;
using System;
using System.ServiceProcess;

namespace FortuneTellerWinService
{
    public partial class FortuneService : ServiceBase
    {
        private const string _baseAddress = "http://localhost:5002/";
        private IDisposable _server = null;

        public FortuneService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _server = WebApp.Start<Startup>(url: _baseAddress);
        }

        protected override void OnStop()
        {
            if (_server != null)
            {
                _server.Dispose();
            }

            base.OnStop();
        }
    }
}
