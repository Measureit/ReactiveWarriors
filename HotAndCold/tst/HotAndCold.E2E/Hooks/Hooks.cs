using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace HotAndCold.E2E.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private static IHost _host;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _host = Program.CreateHostBuilder(null).Build();

            _host.Start();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _host.StopAsync().Wait();
        }
    }
}
