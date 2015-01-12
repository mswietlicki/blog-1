var Main;
(function (Main) {
    var DiagramPointModel = (function () {
        function DiagramPointModel() {
        }
        Object.defineProperty(DiagramPointModel.prototype, "Y", {
            get: function () {
                return this._Y;
            },
            set: function (id) {
                this._Y = id;
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(DiagramPointModel.prototype, "X", {
            get: function () {
                return this._X;
            },
            set: function (id) {
                this._X = id;
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(DiagramPointModel.prototype, "LineId", {
            get: function () {
                return this._LineId;
            },
            set: function (id) {
                this._LineId = id;
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(DiagramPointModel.prototype, "DiagramId", {
            get: function () {
                return this.DiagramId;
            },
            set: function (id) {
                this._DiagramId = id;
            },
            enumerable: true,
            configurable: true
        });
        return DiagramPointModel;
    })();
    Main.DiagramPointModel = DiagramPointModel;
})(Main || (Main = {}));
//# sourceMappingURL=DiagramPointModel.js.map
