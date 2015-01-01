using System;
using System.Configuration;
using System.Diagnostics;
using Sychev.AppFabricMonitoring.StreamInsightServer.Contract;
using Sychev.Monitoring.StreamInsight.ObservableServiceProxy.NonValueProxy;
using Sychev.Monitoring.StreamInsight.ObservableServiceProxy.ValueEventProxy;
using NonValueEvent = Sychev.Monitoring.StreamInsight.ObservableServiceProxy.ValueEventProxy.NonValueEvent;

namespace Sychev.AppFabricMonitoring.Generator.Tracking
{
    class Program
    {
        static void Main(string[] args)
        {

            var totalCounter = Convert.ToInt32(ConfigurationManager.AppSettings["TotalNumberOfRequests"]);

            var counter = 1;
            var stopWatch = new Stopwatch();

            Console.WriteLine("Press 1 if ValueEvent. Press 0 if Event withour value");
            var mode = Convert.ToInt32(Console.ReadLine());

            if (mode == 0)
            {
                stopWatch.Start();
                var connection = new WCFDataSourceOf_NonValueEventClient();
                while (totalCounter != 0)
                {
                    connection.PushEvent(new NonValueEvent
                    {
                        Name = "Test",
                        Time = DateTime.Now,
                        TrackEventType = (int)TrackEventType.InstanseStart
                    });

                    totalCounter--;
                    Console.WriteLine(counter);
                    counter++;
                }
            }
            else
            {
                stopWatch.Start();
                var connection = new WCFDataSourceOf_ValueEventClient();
                var randon = new Random();
                while (totalCounter != 0)
                {
                    connection.PushEvent(new ValueEvent
                    {
                        Name = "Test",
                        Time = DateTime.Now,
                        TrackEventType = (int)TrackEventType.InstanseStart,
                        Value = Math.Round((randon.NextDouble() * 50), 5)
                    });

                    totalCounter--;
                    Console.WriteLine(counter);
                    counter++;
                }
            }

            stopWatch.Stop();
            Console.WriteLine("EllapsedTime: {0}", stopWatch.Elapsed);
            Console.ReadLine();
        }
    }
}
