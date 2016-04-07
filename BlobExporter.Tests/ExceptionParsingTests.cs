using System;
using System.Linq;
using BlobExporter.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace BlobExporter.Tests
{
    [Trait("Category", "Unit")]
    public class ExceptionParsingTests
    {
        [Fact]
        public void WhenParsingExceptionJson_ThenObjectPropertiesAsExpected()
        {
            var json = SampleReader.ReadSample("Exception.json");
            var exceptionInfo = ExceptionInfoJsonParser.Parse(json);
            exceptionInfo.Should().NotBeNull();

            exceptionInfo.EventTime.Should().Be(DateTime.Parse("2016-03-27T21:09:45.3540931Z"));

            exceptionInfo.ExceptionStacks.Count().Should().Be(1);
            var exceptionStack = exceptionInfo.ExceptionStacks.Single();
            exceptionStack.Message.Should().Be("Test error");

            exceptionStack.StackTrace.Should().NotBeNull();
            exceptionStack.StackTrace.Count().Should().Be(17);
            var topLevel = exceptionStack.StackTrace.First();
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

        [Fact]
        public void WhenParsingExceptionWithInnerException_ThenStackTracesAsExpected()
        {
            var json = SampleReader.ReadSample("ExceptionWithInnerException.json");
            var exceptionInfo = ExceptionInfoJsonParser.Parse(json);
            exceptionInfo.Should().NotBeNull();

            var stacks = exceptionInfo.ExceptionStacks.ToList();
            stacks.Count().Should().Be(2);

            var outerStack = stacks[0];
            outerStack.Message.Should().Be("Test error");
            outerStack.StackTrace.Should().NotBeNull();
            outerStack.StackTrace.Count().Should().Be(20);
            var outerStackTopLevel = outerStack.StackTrace.First();
            outerStackTopLevel.Method.Should().Be("SampleWebApp.Controllers.TestErrorController.Index");
            outerStackTopLevel.Assembly.Should().Be("SampleWebApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            outerStackTopLevel.FileName.Should().Be(@"C:\Code\AppInsightsExporter\SampleWebApp\Controllers\TestErrorController.cs");
            outerStackTopLevel.LineNumber.Should().Be(19);

            var innerStack = stacks[1];
            innerStack.Message.Should().Be("Attempted to divide by zero.");
            innerStack.StackTrace.Should().NotBeNull();
            innerStack.StackTrace.Count().Should().Be(2);
            var innerStackTopLevel = innerStack.StackTrace.First();
            innerStackTopLevel.Method.Should().Be("System.Math.DivRem");
            innerStackTopLevel.Assembly.Should().Be("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            innerStackTopLevel.FileName.Should().BeNull();
            innerStackTopLevel.LineNumber.Should().Be(0);
        }
    }
}
