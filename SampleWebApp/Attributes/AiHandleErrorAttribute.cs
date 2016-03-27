using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace SampleWebApp.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AiHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
            {
                //If customError is Off, then AI HTTPModule will report the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    // Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
                    var ai = new TelemetryClient();

                    var properties = new Dictionary<string, string>
                    {
                        {"testProperty0", "testValue0"},
                        {"testProperty1", "testValue1"}
                    };
                    var metrics = new Dictionary<string, double>
                    {
                        {"testMetric0", -1.2345678},
                        {"testMetric1", 3.1415926}
                    };

                    ai.TrackException(filterContext.Exception, properties, metrics);
                }
            }
            base.OnException(filterContext);
        }
    }
}