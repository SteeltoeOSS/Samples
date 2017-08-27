using Steeltoe.CircuitBreaker.Hystrix.Strategy.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FortuneTellerService4
{
    public class HystrixRequestContextModule : IHttpModule
    {
        public void Dispose()
        {
           
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app != null)
            {
                var items = app.Context?.Items;
                if (items != null)
                {
                    var hystrix = items["_hystrix_context_"] as HystrixRequestContext;
                    if (hystrix != null)
                    {
                        hystrix.Dispose();
                    }
                }
            }
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app != null)
            {
                var items = app.Context?.Items;
                if (items != null)
                {
                    items["_hystrix_context_"] = HystrixRequestContext.InitializeContext();
                }
            }
           
        }

    }
}