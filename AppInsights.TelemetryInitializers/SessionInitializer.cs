using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppInsights.TelemetryInitializers
{
    /// <summary>Add a session to the context of every telemetry.</summary>
    /// <remarks>
    /// This is useful in non-web application (i.e. where no session is added by ASP.NET).
    /// It allows to segregate different run of an executable for instance.
    /// </remarks>
    public class SessionInitializer : ITelemetryInitializer
    {
        private readonly string _id;

        /// <summary>Construct an initializer with a random (GUID) session ID.</summary>
        public SessionInitializer():this(Guid.NewGuid().ToString())
        {
        }

        /// <summary>Construct an initializer with a session ID.</summary>
        /// <param name="id">Session ID to attach to telemetry's context.</param>
        public SessionInitializer(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            _id = id;
        }

        void ITelemetryInitializer.Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Session.Id = _id;
        }
    }
}