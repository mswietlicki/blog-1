var Main;
(function (Main) {
    //точка с 2 свойствами. x,y
    var Point = (function () {
        function Point(x, y) {
            this.x = x;
            this.y = y;
        }
        return Point;
    })();
    Main.Point = Point;
})(Main || (Main = {}));
//# sourceMappingURL=Point.js.map
