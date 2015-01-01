var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="../../signalr/signalr.d.ts" />
/// <reference path="../../jquery/jquery.d.ts" />
var Main;
(function (Main) {
    var IndexPageModel = (function (_super) {
        __extends(IndexPageModel, _super);
        function IndexPageModel() {
            _super.call(this);
            this._diagrams = new Array();
        }
        //точка входа. В конструкторе это делать не правильно.
        IndexPageModel.prototype.init = function () {
            var _this = this;
            //SignalR connection
            this._hubConnection = $.connection;

            //SignalR Hub Proxy
            this._diagramHub = this._hubConnection.DiagramHub;

            //подписываемся на сервереное событие diagramNotify
            this._diagramHub.client.diagramNotify = function (data) {
                _this.onUpdateRecieve(data);
            };

            //console.log("Start Connection to hub");
            this._hubConnection.hub.start().done(function () {
                //после подключения к hub, получить список диаграмм от api
                $.get("/api/Diagram/GetDiagrams").done(function (data) {
                    _this.renderHeader(data);

                    _this.onGetDiagrams(data);
                });
            });
        };
        return IndexPageModel;
    })(Main.BasePageModel);
    Main.IndexPageModel = IndexPageModel;
})(Main || (Main = {}));
//# sourceMappingURL=IndexPageModel.js.map
