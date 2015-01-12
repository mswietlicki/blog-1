using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace Sychev.Monitoring.Web.Hubs
{
    public class ErrorHandlingPipelineModule : HubPipelineModule
    {
        public ErrorHandlingPipelineModule()
        {
            
        }

        protected override void OnIncomingError(ExceptionContext exceptionContext,
            IHubIncomingInvokerContext invokerContext)
        {
            base.OnIncomingError(exceptionContext,invokerContext);
        }

        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            return base.OnBeforeIncoming(context);
        }
        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            return base.OnBeforeOutgoing(context);
        }

        protected override object OnAfterIncoming(object result, IHubIncomingInvokerContext context)
        {
            return base.OnAfterIncoming(result, context);
        }

        public override Func<IHubIncomingInvokerContext, Task<object>> BuildIncoming(Func<IHubIncomingInvokerContext, Task<object>> invoke)
        {
            return base.BuildIncoming(invoke);
        }
    }
}