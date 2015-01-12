/// <reference path="../../signalr/signalr.d.ts" />
/// <reference path="../../jquery/jquery.d.ts" />
var Main;
(function (Main) {
    var BasePageModel = (function () {
        function BasePageModel() {
            this._diagrams = new Array();
        }
        //действие на получение списка диаграмм
        BasePageModel.prototype.onGetDiagrams = function (data) {
            var _this = this;
            //получив список диаграмм, для каждой из них выполняем подключение к соответсвующей группе в hub.
            data.Data.forEach(function (dataElement) {
                _this.joinGroup(dataElement);
            });
        };

        //действие для каждой диаграммы- подключение к Group.
        BasePageModel.prototype.joinGroup = function (diagram) {
            var _this = this;
            //создаем визуальное представление диаграммы.
            var dl = new Main.DiagramViewModel(diagram);

            //сохраняем его себе.
            this._diagrams.push(dl);

            //получаем исторические данные, за последние сколько-то минут, до того как начинаем получать push нотификации.
            $.get(("/api/Diagram/GetDiagramData/" + diagram.Id)).done(function (data) {
                var localData = data;

                _this.onUpdateRecieve(localData);
            });

            //вызываем метод hub подключиться к группе, чтобы получать нотификации.
            this._diagramHub.server.joinGroup(diagram.Id).done(function () {
                //console.log("JoinGroup");
            });
        };

        BasePageModel.prototype.renderHeader = function (data) {
            $("body").append("<header class='headerContainer'>" + "</header>");
            $("header").append("<a href=\"/home/index/\">Home</a>");
            data.Data.forEach(function (dataElement) {
                $("header").append("<a href=\"/home/chart/" + dataElement.Id + "\">" + dataElement.Name + "</a>");
            });
        };

        //пришло с signalr hub сообщение, что появились новые точки для отрисовки.
        //или мы получили исторические данные из controller.
        BasePageModel.prototype.onUpdateRecieve = function (data) {
            //console.log("newData");
            var _this = this;
            //добавляем каждую точку к соответсвующей диаграмме
            data.Data.forEach(function (diagram) {
                var diagramId = diagram.DiagramId;

                var localDiagram;

                for (var i = 0; i < _this._diagrams.length; i++) {
                    if (_this._diagrams[i].Id == diagramId) {
                        localDiagram = _this._diagrams[i];
                        break;
                    }
                }
                if (localDiagram == undefined) {
                    console.log("Diagram Not Find");
                    return;
                }

                //добавляем все точки, которые относятся к этой диаграмме.
                diagram.Points.forEach(function (point) {
                    localDiagram.addNewPoint(point);
                });

                //рендерим диаграмму заново.
                localDiagram.render();
            });
        };

        BasePageModel.prototype.close = function (diagramId) {
            //ищем диаграмму
            var diagra = this.getDiagramAndIndex(diagramId);
            var diagramIndex = diagra.diagramIndex;
            var localDiagram = diagra.localDiagram;

            //если не нашли, идем на выход
            if (diagramIndex == -1 || localDiagram == undefined) {
                return;
            }

            //закрываем диаграмму из viewmodel
            this._diagramHub.server.leftGroup(diagramId);

            //удаляем из dom дерева
            localDiagram.close();

            //удаляем диаграмму из массива
            this._diagrams.slice(diagramIndex, 1);
        };

        BasePageModel.prototype.changeDiagramType = function (diagramId) {
            //ищем диаграмму
            var diagra = this.getDiagramAndIndex(diagramId);
            var diagramIndex = diagra.diagramIndex;
            var localDiagram = diagra.localDiagram;

            //если не нашли, идем на выход
            if (diagramIndex == -1 || localDiagram == undefined) {
                return;
            }

            //меняеем тип
            localDiagram.changeDiagramType();
        };

        BasePageModel.prototype.onChangeDataCollectingClick = function (diagramId) {
            var diagra = this.getDiagramAndIndex(diagramId);
            var diagramIndex = diagra.diagramIndex;
            var localDiagram = diagra.localDiagram;

            //если не нашли, идем на выход
            if (diagramIndex == -1 || localDiagram == undefined) {
                return;
            }
            var isReconect = localDiagram.onChangeDataCollectingClick();

            if (!isReconect) {
                //вызываем метод hub переподключения к группе, чтобы получать нотификации.
                this._diagramHub.server.leftGroup(diagramId).done(function () {
                    //console.log("leftGroup");
                });
            } else {
                //вызываем метод hub переподключения к группе, чтобы получать нотификации.
                this._diagramHub.server.joinGroup(diagramId).done(function () {
                    //console.log("joinGroup");
                });
            }
        };

        BasePageModel.prototype.getDiagramAndIndex = function (diagramId) {
            var diagramIndex = -1;
            var localDiagram = null;

            for (var i = 0; i < this._diagrams.length; i++) {
                if (this._diagrams[i].Id == diagramId) {
                    diagramIndex = i;
                    localDiagram = this._diagrams[i];
                    break;
                }
            }
            return { localDiagram: localDiagram, diagramIndex: diagramIndex };
        };
        return BasePageModel;
    })();
    Main.BasePageModel = BasePageModel;
})(Main || (Main = {}));
//# sourceMappingURL=BasePageModel.js.map
