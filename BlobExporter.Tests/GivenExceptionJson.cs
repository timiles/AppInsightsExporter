using System;
using System.Linq;
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
            var exceptionInfo = ExceptionInfoJsonParser.Parse(this._json);
            exceptionInfo.Should().NotBeNull();

            exceptionInfo.EventTime.Should().Be(DateTime.Parse("2016-03-27T21:09:45.3540931Z"));

            exceptionInfo.Message.Should().Be("Test error");

            exceptionInfo.StackTrace.Should().NotBeNull();
            exceptionInfo.StackTrace.Count().Should().Be(17);
            var topLevel = exceptionInfo.StackTrace.First();
            topLevel.Method.Should().Be("SampleWebApp.Controllers.TestErrorController.Index");
            topLevel.Assembly.Should().Be("SampleWebApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            topLevel.FileName.Should().Be(@"c:\Code\AppInsightsExporter\SampleWebApp\Controllers\TestErrorController.cs");
            topLevel.LineNumber.Should().Be(10);

            exceptionInfo.UserId.Should().Be("test_user");

            // properties
            exceptionInfo.Properties.Should().ContainKey("testProperty0");
            exceptionInfo.Properties["testProperty0"].Should().Be("testValue0");

            exceptionInfo.Properties.Should().ContainKey("testProperty1");
            exceptionInfo.Properties["testProperty1"].Should().Be("testValue1");

            // metrics
            exceptionInfo.Metrics.Should().ContainKey("testMetric0");
            exceptionInfo.Metrics["testMetric0"].Should().Be(-1.2345678);

            exceptionInfo.Metrics.Should().ContainKey("testMetric1");
            exceptionInfo.Metrics["testMetric1"].Should().Be(3.1415926);
        }
    }
}
