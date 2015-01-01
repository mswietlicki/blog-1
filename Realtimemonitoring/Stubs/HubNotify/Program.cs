using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Sychev.Monitoring.Web.Contract.Models.Incoming;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.AppFabricMonitoring.Generator.HubNotify
{
    class Program
    {
        static void Main(string[] args)
        {

            var hubUri = ConfigurationManager.AppSettings["HubUri"];
            var totalCounter = Convert.ToInt32(ConfigurationManager.AppSettings["TotalNumberOfRequests"]);
            var counter = 1;

            var hubConnection = new HubConnection(hubUri);
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("DiagramHub");
            hubConnection.Start().Wait();

            var firstDiagramId = Guid.Parse("0AF02C07-A9B5-4845-B856-629B2F983F43");
            var firstDFirstLineId = Guid.Parse("AB0B4D11-525F-4FC7-AB0C-90EE3BA800AF");


            var secondDiagramId = Guid.Parse("28319487-F922-49F7-8EB2-03F276568149");
            var secondDFirstLineId = Guid.Parse("A48A208A-6DB5-48D7-8519-A15F1941FCE2");
            var secondDSecondLineId = Guid.Parse("8B1262FF-3453-4259-B2AD-22D4A9B3DDBE");
            var secondDThirdLineId = Guid.Parse("B3274645-CDBD-4B8B-B6AC-CF5B352DE047");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while (totalCounter != 0)
            {
                Thread.Sleep(500);
                int counter1 = counter;
                var random = new Random();
                var task1 = new Task(() =>
                {
                    var data2 = new BroadCastDiagramModel
                    {
                        Points = new [] {  new DiagramPointModel
                        {
                            LineId = firstDFirstLineId,
                            X = DateTime.UtcNow,
                            Y = random.Next(100) + Math.Pow(-1, counter1),
                            DiagramId = firstDiagramId
                        }}
                    };
                    stockTickerHubProxy.Invoke("BroadCastDiagram", data2).Wait();
                });
                task1.Start();

                var task2 = new Task(() =>
                {
                    var data2 = new BroadCastDiagramModel
                    {
                        Points = new[] { 
                            new DiagramPointModel 
                            { 
                                LineId = secondDFirstLineId,
                                X = DateTime.UtcNow,
                                Y =  random.Next(50)+Math.Pow(-1,counter1),
                                DiagramId = secondDiagramId
                            },
                            new DiagramPointModel 
                            { 
                                LineId = secondDSecondLineId,
                                X = DateTime.UtcNow,
                                Y =  random.Next(50)+Math.Pow(-1,counter1),
                                DiagramId = secondDiagramId
                            },
                            new DiagramPointModel 
                            { 
                                LineId = secondDThirdLineId,
                                X = DateTime.UtcNow,
                                Y =  random.Next(50)+Math.Pow(-1,counter1),
                                DiagramId = secondDiagramId
                            }
                        }
                    };
                    //stockTickerHubProxy.Invoke("BroadCastDiagram", data2).Wait();
                });
                task2.Start();
                Task.WaitAll(task1, task2);

                totalCounter--;
                Console.WriteLine(counter);
                counter++;
            }
            stopWatch.Stop();
            Console.WriteLine("EllapsedTime: {0}", stopWatch.Elapsed);
            Console.ReadLine();
        }
    }
}
