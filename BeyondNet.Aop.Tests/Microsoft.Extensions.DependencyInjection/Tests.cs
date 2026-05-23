using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeyondNet.Aop.Aspects.Installer;
using Serilog;
using BeyondNet.Aop.Microsoft.Extensions.DependencyInjection.Aspects.Installer;
using Microsoft.Extensions.DependencyInjection;
using BeyondNet.Aop.Aspects.Logger.Serilog;

namespace BeyondNet.Aop.Tests.Microsoft.Extensions.DependencyInjection
{
    [TestClass]
    public class Tests
    {

        [TestMethod]
        public void Proxy_WithOneAspect_ShoudBe()
        {
            var container = new ServiceCollection();

            container.AddSingleton<INumberProvider, NumberProvider>();

            container.AddAop(c=>
            {
                c.AddAspect<Add>();
            });

            

            var p = container.BuildServiceProvider();

            var provider = p.GetService<INumberProvider>();

            var test = new TestCases();

            test.Proxy_WithOneAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithMultipleAspect_ShoudBe()
        {
            var container = new ServiceCollection();

            container.AddSingleton<INumberProvider, NumberProvider>();

            container.AddAop(c =>
            {
                c.AddAspect<Add10>();
                c.AddAspect<Multiple5>();
                c.AddAspect<Subtract20>();
            });

            

            var p = container.BuildServiceProvider();

            var provider = p.GetService<INumberProvider>();

            var test = new TestCases();

            test.Proxy_WithMultipleAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithAdviceAspect_ShoudBe()
        {
            var container = new ServiceCollection();

            container.AddSingleton<INumberProvider, NumberProvider>();

            container.AddAop(action: c => { 
                c.AddAdvice<AddAdvice>();
            });

            var p = container.BuildServiceProvider();

            var provider = p.GetService<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithAdviceAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithLogAspect_ShoudBe()
        {
            var container = new ServiceCollection();

            container.AddSingleton<INumberProvider, NumberProvider>();

            container.AddAop(action: c =>
            {
                c.AddLogger<SerilogLogger>();
            });

            

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}").MinimumLevel.Verbose()
            .CreateLogger();

            var p = container.BuildServiceProvider();

            var provider = p.GetService<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithLogAspect_ShoudBe(provider);
        }
    }
}
