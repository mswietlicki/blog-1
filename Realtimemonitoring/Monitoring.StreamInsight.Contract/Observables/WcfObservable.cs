using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Sychev.Monitoring.StreamInsight.Contract.Observables
{
    public class WCFObservable<T> : IObservable<T>
    {
        private readonly string _baseAddress;
        private readonly string _name;
        private readonly List<IObserver<T>> _observers;
        private readonly ServiceHost _selfHost;

        public WCFObservable(string baseAddress, string name)
        {
            _baseAddress = baseAddress;
            _name = name;
            _observers = new List<IObserver<T>>();
            _selfHost = new ServiceHost(new WCFDataSource<T>(this), new Uri(baseAddress));

            //если нужно будет добавить performance модно сменить binding тут. Сычев Игорь 12 марта 2013
            var binding = new BasicHttpBinding
            {
            };

            _selfHost.AddServiceEndpoint(typeof(IWCFDataSource<T>), binding, name);

            _selfHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_observers.Count == 0 && _selfHost.State != CommunicationState.Opened)
            {
                _selfHost.Open();
            }

            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber(_observers, observer, _selfHost);
        }

        public void OnNext(T value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;
            private readonly ServiceHost _selfHost;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer, ServiceHost selfHost)
            {
                _observers = observers;
                _observer = observer;
                _selfHost = selfHost;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
                if (_observers.Count == 0)
                {
                    _selfHost.Close();
                }
            }
        }
    }
}
