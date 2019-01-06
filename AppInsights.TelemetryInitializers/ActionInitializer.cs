using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppInsights.TelemetryInitializers
{
    /// <summary>Generic initializer invoking an action at initialization.</summary>
    public class ActionInitializer : ITelemetryInitializer
    {
        private readonly Action<TelemetryContext> _action;

        /// <summary>Construct an initializer with an action.</summary>
        /// <param name="action">Action to invoke when initilizing a telemetry's context.</param>
        public ActionInitializer(Action<TelemetryContext> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        void ITelemetryInitializer.Initialize(ITelemetry telemetry)
        {
            _action(telemetry.Context);
        }
    }
}