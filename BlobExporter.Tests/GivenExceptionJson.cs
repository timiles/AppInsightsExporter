using System.Linq;
using BlobExporter.Models;
using BlobExporter.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace BlobExporter.Tests
{
    [Trait("Category", "Unit")]
    public class GivenExceptionJson
    {
        private readonly string _json;

        public GivenExceptionJson()
        {
            this._json = SampleReader.ReadSample("Exception.json");
        }

        [Fact]
        public void WhenParsed_ThenObjectPropertiesAsExpected()
        {
            var exceptionTelemetry = TelemetryJsonParser.Parse(this._json);
            exceptionTelemetry.Should().NotBeNull();

            exceptionTelemetry.Message.Should().Be("Test error");

            exceptionTelemetry.StackTrace.Should().NotBeNull();
            exceptionTelemetry.StackTrace.Count().Should().Be(17);
            var topLevel = exceptionTelemetry.StackTrace.First();
            topLevel.Method.Should().Be("SampleWebApp.Controllers.TestErrorController.Index");
            topLevel.Assembly.Should().Be("SampleWebApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            topLevel.FileName.Should().Be(@"c:\Code\AppInsightsExporter\SampleWebApp\Controllers\TestErrorController.cs");
            topLevel.LineNumber.Should().Be(10);

            exceptionTelemetry.UserId.Should().Be("test_user");

            // properties
            exceptionTelemetry.Properties.Should().ContainKey("testProperty0");
            exceptionTelemetry.Properties["testProperty0"].Should().Be("testValue0");

            exceptionTelemetry.Properties.Should().ContainKey("testProperty1");
            exceptionTelemetry.Properties["testProperty1"].Should().Be("testValue1");

            // metrics
            exceptionTelemetry.Metrics.Should().ContainKey("testMetric0");
            exceptionTelemetry.Metrics["testMetric0"].Should().Be(-1.2345678);

            exceptionTelemetry.Metrics.Should().ContainKey("testMetric1");
            exceptionTelemetry.Metrics["testMetric1"].Should().Be(3.1415926);
        }
    }
}
