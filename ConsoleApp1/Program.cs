using jsreport.Binary;
using jsreport.Local;
using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var rs = new LocalReporting()
                         .UseBinary(JsReportBinary.GetBinary())
                         .KillRunningJsReportProcesses()
                         .RunInDirectory(Path.Combine(Directory.GetCurrentDirectory(), "jsreport"))
                         .Configure(cfg => cfg.CreateSamples()
                            .FileSystemStore())
                         .AsWebServer()
                         .RedirectOutputToConsole()
                         .Create();

            rs.StartAsync().Wait();

            Process.Start(new ProcessStartInfo("cmd", $"/c start http://localhost:5488"));

            Console.ReadKey();

            rs.KillAsync().Wait();
        }
    }
}
