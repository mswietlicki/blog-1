var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="ResponseTemplate.ts" />
var Main;
(function (Main) {
    var GenericResponseModel = (function (_super) {
        __extends(GenericResponseModel, _super);
        function GenericResponseModel() {
            _super.call(this);
        }
        Object.defineProperty(GenericResponseModel.prototype, "Data", {
            get: function () {
                return this._data;
            },
            set: function (data) {
                this._data = data;
            },
            enumerable: true,
            configurable: true
        });
        return GenericResponseModel;
    })(Main.ResponceTemplate);
    Main.GenericResponseModel = GenericResponseModel;
})(Main || (Main = {}));
//# sourceMappingURL=GenericResponseModel.js.map
