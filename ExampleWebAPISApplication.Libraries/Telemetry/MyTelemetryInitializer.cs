using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace ExampleWebAPISApplication.Libraries.Telemetry
{
    public class MyTelemetryInitializer : ITelemetryInitializer, ITelemetryModule
    {
        private readonly string loggingNamespace;
        private readonly string instrumentationKey;

        public MyTelemetryInitializer(string loggingNamespace, string instrumentationKey)
        {
            this.loggingNamespace = loggingNamespace;
            this.instrumentationKey = instrumentationKey;
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is null)
                throw new ArgumentNullException(nameof(telemetry));

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = loggingNamespace;
                telemetry.Context.InstrumentationKey = instrumentationKey;
            }
        }

        public void Initialize(TelemetryConfiguration configuration)
        {
            configuration.TelemetryProcessorChainBuilder.Build();
        }
    }
}
