/// <reference path="../jquery/jquery.d.ts" />
/// <reference path="../signalr/signalr.d.ts" />
/// <reference path="Incoming/Responce/GenericPushModel.ts" />
/// <reference path="Incoming/PushDiagram/UpdateDiagramModel.ts" />

interface IDiagramClient {
	diagramNotify(data: Main.GenericPushModel<Main.UpdateDiagramModel>);
}
 
interface IDiagramServer {
    joinGroup(id: string): JQueryPromise<void>;
    leftGroup(id: string): JQueryPromise<void>;
}
 
interface HubProxy {
	client: IDiagramClient;
	server: IDiagramServer;
}
 
interface SignalR {
    DiagramHub: HubProxy;
} 