using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.Web.Code.Repository
{
    public class PointsRepository : IPointsRepository
    {
        private static readonly ConcurrentDictionary<Guid, ConcurrentQueue<DiagramPointModel>> Data = new ConcurrentDictionary<Guid, ConcurrentQueue<DiagramPointModel>>();

        private const int MaxCount = 10000;
        private static readonly TimeSpan MaxLiveTime = new TimeSpan(0, 1, 0, 0);


        public List<DiagramPointModel> GetPoinsByDiagram(Guid id)
        {
            ConcurrentQueue<DiagramPointModel> queue;
            
            Data.TryGetValue(id, out queue);

            if (queue != null)
            {
                var toRet = queue.ToList();

                return toRet;
            }
            else
            {
                return new List<DiagramPointModel>();
            }
        }

        public void PushNewPoints(Guid id, IEnumerable<DiagramPointModel> newPoints)
        {
            if (!Data.ContainsKey(id))
            {
                Data.TryAdd(id, new ConcurrentQueue<DiagramPointModel>());
            }
            ConcurrentQueue<DiagramPointModel> queue = Data[id];
            var currentTime = DateTime.UtcNow;

            //вставляем новые точки.
            foreach (var diagramPointModel in newPoints)
            {
                queue.Enqueue(diagramPointModel);
            }



            CleanUpQueue(queue, currentTime);
        }

        private static void CleanUpQueue(ConcurrentQueue<DiagramPointModel> queue, DateTime currentTime)
        {
            //удаляем все старые
            while (true)
            {
                var currentCount = queue.Count;

                DiagramPointModel model;

                queue.TryPeek(out model);
                if (model != null)
                {
                    var diff = currentTime - model.X;
                    if (currentCount > MaxCount || diff > MaxLiveTime)
                    {
                        queue.TryDequeue(out model);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}