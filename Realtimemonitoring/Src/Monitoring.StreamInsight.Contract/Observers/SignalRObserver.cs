using System;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Sychev.Monitoring.Web.Contract.Models.Incoming;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.StreamInsight.Contract.Observers
{
    public class SignalRObserver : IObserver<DiagramModelCollection>
    {
        private readonly string _baseAddress;
        private HubConnection _hubConnection;
        private IHubProxy _stockTickerHubProxy;
        public SignalRObserver(string baseAddress)
        {
            _baseAddress = baseAddress;
            try
            {
                _hubConnection = new HubConnection(baseAddress);
                //_hubConnection.JsonSerializer.FloatFormatHandling = FloatFormatHandling.String;
                _stockTickerHubProxy = _hubConnection.CreateHubProxy("DiagramHub");
                _hubConnection.Start().Wait();
            }
            catch(Exception ex)
            {
                Debugger.Break();
                Console.WriteLine(ex.Message);
            }
        }

        public void OnCompleted()
        {
            _hubConnection.Stop();
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
            _hubConnection.Stop();
        }

        public void OnNext(DiagramModelCollection data)
        {
            if (_hubConnection.State != ConnectionState.Connected)
            {
                _hubConnection.Start().Wait();
            }
            //костыль. streamInsight не поддерживает массивы. А без них совсем тяжко
            //http://technet.microsoft.com/en-us/library/ee842720.aspx
            //http://technet.microsoft.com/en-us/library/ee378905.aspx
            //http://social.msdn.microsoft.com/Forums/en-US/07a702a5-2cf1-45d4-add7-572d81daeedd/can-streaminsight-handle-structured-eventsmessages?forum=streaminsight
            //http://social.msdn.microsoft.com/Forums/sqlserver/en-US/b16002dc-2c87-4f2c-acfd-d2d40b7ce787/simulating-collections-using-multiple-streams?forum=streaminsight
           
            var diagramPoints = JsonConvert.DeserializeObject<DiagramPointModel[]>(data.Data);
            var point = new BroadCastDiagramModel
            {
                Points = diagramPoints
            };

            _stockTickerHubProxy.Invoke("BroadCastDiagram", point).Wait();
        }
    }

}
