using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace ApplicationTracker.Core.IntegrationTests
{
    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper output;

        public XUnitLoggerProvider(ITestOutputHelper output)
        {
            this.output = output;
        }

        public ILogger CreateLogger<T>() => new XUnitLogger<T>(output);

        public ILogger CreateLogger(string categoryName)
        {
            return CreateLogger<XUnitLoggerProvider>();
        }

        public void Dispose() { }
    }
}
