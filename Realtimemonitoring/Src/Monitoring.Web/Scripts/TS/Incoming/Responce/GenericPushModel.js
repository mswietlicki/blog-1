var Main;
(function (Main) {
    var GenericPushModel = (function () {
        function GenericPushModel() {
        }
        Object.defineProperty(GenericPushModel.prototype, "Data", {
            get: function () {
                return this._data;
            },
            set: function (data) {
                this._data = data;
            },
            enumerable: true,
            configurable: true
        });
        return GenericPushModel;
    })();
    Main.GenericPushModel = GenericPushModel;
})(Main || (Main = {}));
//# sourceMappingURL=GenericPushModel.js.map
