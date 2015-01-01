var Main;
(function (Main) {
    var UpdateDiagramModel = (function () {
        function UpdateDiagramModel() {
            this._points = new Array();
        }
        Object.defineProperty(UpdateDiagramModel.prototype, "DiagramId", {
            get: function () {
                return this._diagramId;
            },
            set: function (name) {
                this._diagramId = name;
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(UpdateDiagramModel.prototype, "Points", {
            get: function () {
                return this._points;
            },
            set: function (diagrams) {
                this._points = diagrams;
            },
            enumerable: true,
            configurable: true
        });
        return UpdateDiagramModel;
    })();
    Main.UpdateDiagramModel = UpdateDiagramModel;
})(Main || (Main = {}));
//# sourceMappingURL=UpdateDiagramModel.js.map
