/// <reference path="Point.ts" />
/// <reference path="Incoming/Shared/DiagramLineModel.ts" />
/// <reference path="Incoming/Shared/DiagramPointModel.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Main;
(function (Main) {
    //линия с точками
    var DiagramLineWithPointsModel = (function (_super) {
        __extends(DiagramLineWithPointsModel, _super);
        function DiagramLineWithPointsModel(id, name) {
            _super.call(this);
            this.Id = id;
            this.Name = name;
            this._points = new Array();
        }
        Object.defineProperty(DiagramLineWithPointsModel.prototype, "Points", {
            get: function () {
                return this._points;
            },
            enumerable: true,
            configurable: true
        });
        return DiagramLineWithPointsModel;
    })(Main.DiagramLineModel);
    Main.DiagramLineWithPointsModel = DiagramLineWithPointsModel;
})(Main || (Main = {}));
//# sourceMappingURL=DiagramLineWithPointsModel.js.map
