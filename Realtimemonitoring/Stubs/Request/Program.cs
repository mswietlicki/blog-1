using System;
using System.Configuration;
using System.Diagnostics;
using Sychev.Monitoring.Workflow.Example.Proxy.ExampleAppProxy;

namespace Sychev.AppFabricMonitoring.Generator.Request
{
	class Program
	{
		static void Main(string[] args)
		{

            var totalCounter = Convert.ToInt32(ConfigurationManager.AppSettings["TotalNumberOfRequests"]);

            var counter = 1;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while (totalCounter != 0)
            {
				var client = new ServiceClient();
				Console.WriteLine("Start New Instance: {0}", counter);
				var returnValue = client.StartNewInstance(new StartNewInstance { TrackId = counter });
				Console.WriteLine("Return: {0}", returnValue.Value);
                stopWatch.Stop();
                Console.WriteLine(stopWatch.Elapsed);
                //if (stopWatch.Elapsed > new TimeSpan(0, 0, 0, 0, 200))
                //{
                //    Console.WriteLine("Problem is now!");
                //    Console.ReadLine();
                //}
                   
                stopWatch.Restart();
				//Thread.Sleep(1000);



                totalCounter--;
                Console.WriteLine(counter);

                counter++;
			}

            Console.WriteLine("EllapsedTime: {0}", stopWatch.Elapsed);
            Console.ReadLine();
		}
	}
}
