/// <reference path="diagramtype.ts" />
var Main;
(function (Main) {
    var DiagramModel = (function () {
        function DiagramModel() {
        }
        Object.defineProperty(DiagramModel.prototype, "Name", {
            get: function () {
                return this._name;
            },
            set: function (name) {
                this._name = name;
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(DiagramModel.prototype, "Id", {
            get: function () {
                return this._id;
            },
            set: function (id) {
                this._id = id;
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(DiagramModel.prototype, "Lines", {
            get: function () {
                return this._lines;
            },
            set: function (lines) {
                this._lines = lines;
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(DiagramModel.prototype, "DiagramType", {
            get: function () {
                return this._diagramType;
            },
            set: function (diagramType) {
                this._diagramType = diagramType;
            },
            enumerable: true,
            configurable: true
        });
        return DiagramModel;
    })();
    Main.DiagramModel = DiagramModel;
})(Main || (Main = {}));
//# sourceMappingURL=DiagramModel.js.map
