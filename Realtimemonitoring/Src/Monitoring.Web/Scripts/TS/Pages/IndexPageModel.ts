/// <reference path="../../signalr/signalr.d.ts" />
/// <reference path="../../jquery/jquery.d.ts" />
module Main {

    export class IndexPageModel extends BasePageModel {
        constructor() {
            super();
            this._diagrams = new Array<DiagramViewModel>();
        }

        //точка входа. В конструкторе это делать не правильно.
        public init() {
            //SignalR connection
            this._hubConnection = $.connection;
            //SignalR Hub Proxy 
            this._diagramHub = this._hubConnection.DiagramHub;
            //подписываемся на сервереное событие diagramNotify
            this._diagramHub.client.diagramNotify = (data: GenericPushModel<UpdateDiagramModel>): void => {
                this.onUpdateRecieve(data);
            };
            //console.log("Start Connection to hub");
            this._hubConnection.hub.start().done(() => {
                //после подключения к hub, получить список диаграмм от api
                $.get("/api/Diagram/GetDiagrams")
                    .done((data: GenericResponseModel<DiagramModel>) => {

                        this.renderHeader(data);

                        this.onGetDiagrams(data);
                    });
            });
        }
    }
}