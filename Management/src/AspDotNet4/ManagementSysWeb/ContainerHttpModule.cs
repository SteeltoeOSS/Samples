using HttpModuleMagic;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

[assembly: PreApplicationStartMethod(typeof(ContainerHttpModule), "Start")]
namespace HttpModuleMagic
{
    public class ContainerHttpModule : IHttpModule
    {
        public static void Start()
        {
            HttpApplication.RegisterModule(typeof(ContainerHttpModule));
        }

        private Lazy<IEnumerable<IHttpModule>> _modules = new Lazy<IEnumerable<IHttpModule>>(RetrieveModules);

        private static IEnumerable<IHttpModule> RetrieveModules()
        {
            return DependencyResolver.Current.GetServices<IHttpModule>();
        }

        public void Dispose()
        {
            var modules = _modules.Value;
            foreach (var module in modules)
            {
                if (module is IDisposable disposableModule)
                {
                    disposableModule.Dispose();
                }
            }
        }

        public void Init(HttpApplication context)
        {
            var modules = _modules.Value;
            foreach (var module in modules)
            {
                module.Init(context);
            }
        }
    }
}