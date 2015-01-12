var Main;
(function (Main) {
    var DiagramLineModel = (function () {
        function DiagramLineModel() {
        }
        Object.defineProperty(DiagramLineModel.prototype, "Id", {
            get: function () {
                return this._id;
            },
            set: function (id) {
                this._id = id;
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(DiagramLineModel.prototype, "Name", {
            get: function () {
                return this._name;
            },
            set: function (name) {
                this._name = name;
            },
            enumerable: true,
            configurable: true
        });
        return DiagramLineModel;
    })();
    Main.DiagramLineModel = DiagramLineModel;
})(Main || (Main = {}));
//# sourceMappingURL=DiagramLineModel.js.map
