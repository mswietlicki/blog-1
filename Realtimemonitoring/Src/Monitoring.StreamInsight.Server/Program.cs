using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using Microsoft.ComplexEventProcessing.Linq;
using Microsoft.ComplexEventProcessing.ManagementService;
using Sychev.Monitoring.StreamInsight.Contract.Observers;
using Sychev.Monitoring.StreamInsight.Plugin;
using Sychev.Monitoring.StreamInsight.Plugin.NonValueEvent;

namespace Sychev.Monitoring.StreamInsight.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var streamInsightAppName = ConfigurationManager.AppSettings["streamInsightAppName"];
            var streamInsightServerName = ConfigurationManager.AppSettings["streamInsightServerName"];

            var streamInsightPort = ConfigurationManager.AppSettings["streamInsightPort"];

            var serverName = ConfigurationManager.AppSettings["serverName"];


            using (var server = Microsoft.ComplexEventProcessing.Server.Create(streamInsightServerName))
            {

                var host = new ServiceHost(server.CreateManagementService());

                var managementUri = String.Format(@"http://{1}:{0}/StreamInsight/wcf/Management/", streamInsightPort, serverName);
                host.AddServiceEndpoint(typeof(IManagementService), new WSHttpBinding(SecurityMode.Message), managementUri);

                Console.WriteLine("Management URI: {0}", managementUri);
                host.Open();


                //удаляем приложение, если оно уже создано было ранее кем-то.
                if (server.Applications.ContainsKey(streamInsightAppName))
                {
                    server.Applications[streamInsightAppName].Delete();
                }
                var app = server.CreateApplication(streamInsightAppName);

                string wcfSourceUrl = String.Format(@"http://{1}:{0}/StreamInsight/wcf/Source/", streamInsightPort, serverName);

                Console.WriteLine("WCF URI: {0}", wcfSourceUrl);

                //определяем средство доставки данных клиентам.
                string signalRHubUrl = ConfigurationManager.AppSettings["signalRHubUrl"];
                var observableSink = app.DefineObserver(() => new SignalRObserver(signalRHubUrl));

                var sources = new List<IQStreamable<DiagramModelCollection>>();

                var appFabricEventQueue = new QStremableSourceNonValueEventQueue().Get(app, wcfSourceUrl);
                sources.Add(appFabricEventQueue);

                var appFabricEventValueQueue = new QStremableSourceValueEventQueue().Get(app, wcfSourceUrl);
                sources.Add(appFabricEventValueQueue);

                // var observableExceptionEventWcfSource = app.DefineObservable(() => new WCFObservable<SerializableException>(wcfSourceUrl + "ExceptionEventService", "ExceptionEventService"));

                //var query2 = from x in observableExceptionEventWcfSource
                //    .ToPointStreamable(i => PointEvent.CreateInsert<SerializableException>(DateTime.UtcNow, i), AdvanceTimeSettings.IncreasingStartTime)
                //    .TumblingWindow(TimeSpan.FromMilliseconds(30000))
                //             select x.MapEventOnDiagrams();

                //связываем источник и получатель данных
                sources.Bind(observableSink);

                host.Close();
            }
        }
    }

    internal static class Ext
    {
        internal static void Bind(this IReadOnlyList<IQStreamable<DiagramModelCollection>> collection, IRemoteObserver<DiagramModelCollection> remoteObserver, int startIndex = 0)
        {
            if (startIndex == collection.Count)
            {
                Console.WriteLine("Application Start.");
                Console.WriteLine("Press a botton to exit.");
                Console.ReadLine();
            }
            else
            {
                var toBind = collection[startIndex];
                startIndex++;
                using (toBind.Bind(remoteObserver).Run())
                {
                    collection.Bind(remoteObserver, startIndex);
                }
            }
        }
    }
}
