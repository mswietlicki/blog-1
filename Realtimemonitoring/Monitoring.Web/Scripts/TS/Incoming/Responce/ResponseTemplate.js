var Main;
(function (Main) {
    var ResponceTemplate = (function () {
        function ResponceTemplate() {
        }
        Object.defineProperty(ResponceTemplate.prototype, "Success", {
            get: function () {
                return this._success;
            },
            set: function (success) {
                this._success = success;
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(ResponceTemplate.prototype, "Message", {
            get: function () {
                return this._message;
            },
            set: function (message) {
                this._message = message;
            },
            enumerable: true,
            configurable: true
        });
        return ResponceTemplate;
    })();
    Main.ResponceTemplate = ResponceTemplate;
})(Main || (Main = {}));
//# sourceMappingURL=ResponseTemplate.js.map
