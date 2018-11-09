using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace AppInsights.TelemetryInitializers
{
    /// <summary>Add the request body to a telemetry.  Applies to requests telemetries only.</summary>
    /// <remarks>
    /// Warning:  can substantially increase the volume of telemetry.
    /// Based on https://stackoverflow.com/questions/42686363/view-post-request-body-in-application-insights.
    /// </remarks>
    public class RequestBodyInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Construct an initializer.  This is typically done via dependency injection in ASP.NET Core, i.e.
        /// <code>services.AddSingleton&lt;RequestBodyInitializer, RequestBodyInitializer&gt;()</code>
        /// </summary>
        /// <param name="httpContextAccessor">HTTP Context accessor ; passed by depency injection (ASP.NET Core).</param>
        public RequestBodyInitializer(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }
            _httpContextAccessor = httpContextAccessor;
        }

        void ITelemetryInitializer.Initialize(ITelemetry telemetry)
        {
            var requestTelemetry = telemetry as RequestTelemetry;

            if (requestTelemetry != null
                && string.IsNullOrWhiteSpace(requestTelemetry.ResponseCode))
            {
                var request = _httpContextAccessor.HttpContext.Request;
                var method = request.Method;

                if ((method == HttpMethods.Post || method == HttpMethods.Put)
                    && request.Body.CanRead)
                {
                    using (var readStream = new StreamReader(request.Body))
                    {
                        var body = readStream.ReadToEnd();

                        //  Store body in telemetry
                        requestTelemetry.Properties.Add("requestBody", body);
                        //  Reset the stream so data is not lost
                        request.Body = new MemoryStream();
                        using (var writeStream = new StreamWriter(request.Body, UTF8Encoding.ASCII, body.Length, true))
                        {
                            writeStream.Write(body);
                        }
                        request.Body.Position = 0;
                    }
                }
            }
        }
    }
}