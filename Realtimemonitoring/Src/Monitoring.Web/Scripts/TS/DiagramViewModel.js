/// <reference path="Incoming/Shared/DiagramModel.ts" />
/// <reference path="Incoming/Shared/DiagramPointModel.ts" />
/// <reference path="Point.ts" />
/// <reference path="DiagramLineWithPointsModel.ts" />
/// <reference path="../canvasjs/canvasjs.d.ts" />
var Main;
(function (Main) {
    var DiagramViewModel = (function () {
        function DiagramViewModel(diagram) {
            var _this = this;
            this.maxElementForOscilloscope = 1000;
            this._id = diagram.Id;
            this.chartLinesCollection = new Array();
            this._DiagramType = diagram.DiagramType;

            //создаем в dom дереве div в котором будет жить chart
            $("body").append("<div id=\"" + diagram.Id + "\" class=\"chartContainer\"></div>");

            //создаем кнопку закрытия
            var closeButtonTag = "<input id=\"closeButton" + diagram.Id + "\" type=\"button\" value=\"Close\" onclick=\"onDeleteDiagramClick('" + diagram.Id + "')\"/>";

            var changeDiagramTypeTagValue = this._DiagramType == 0 /* Oscilloscope */ ? 'Oscilloscope' : 'Seismograph';
            var changeDiagramTypeTag = "<input id=\"changeButton" + diagram.Id + "\"type=\"button\" value=\"" + changeDiagramTypeTagValue + "\" onclick=\"onChangeDiagramTypeClick('" + diagram.Id + "')\"/>";

            var stopButtonTag = "<input id=\"stopButton" + diagram.Id + "\" type=\"button\" value=\"Stop\" onclick=\"onChangeDataCollectingClick('" + diagram.Id + "')\"/>";

            var stringToAppend = "<div class='bottonContainer' id=\"buttonDiv" + diagram.Id + "\">" + closeButtonTag + changeDiagramTypeTag + stopButtonTag + "</div>";
            $("body").append(stringToAppend);

            //записали все линии в массив линий.
            diagram.Lines.forEach(function (diagramLineModel) {
                _this.chartLinesCollection.push(new Main.DiagramLineWithPointsModel(diagramLineModel.Id, diagramLineModel.Name));
            });

            //инициализируем массивы данных, для отображения в  chart
            var dataArray = [];
            for (var i = 0; i < diagram.Lines.length; i++) {
                var localdataPoint = this.chartLinesCollection[i].Points;
                var lineName = diagram.Lines[i].Name;

                dataArray[i] = {
                    type: "line",
                    xValueType: "dateTime",
                    showInLegend: true,
                    name: lineName,
                    dataPoints: localdataPoint
                };
            }

            //инициализируем сам объект chart.
            this.chart = new CanvasJS.Chart(diagram.Id, {
                zoomEnabled: true,
                title: {
                    text: diagram.Name
                },
                toolTip: {
                    shared: true
                },
                legend: {
                    verticalAlign: "top",
                    horizontalAlign: "center",
                    fontSize: 14,
                    fontWeight: "bold",
                    fontFamily: "calibri",
                    fontColor: "dimGrey",
                    cursor: "pointer",
                    itemclick: function (e) {
                        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                            e.dataSeries.visible = false;
                        } else {
                            e.dataSeries.visible = true;
                        }
                    }
                },
                axisX: {
                    title: "chart updates from server"
                },
                axisY: {
                    prefix: '',
                    includeZero: false
                },
                data: dataArray
            });

            //отображаем получившийся chart.
            this.chart.render();
        }
        Object.defineProperty(DiagramViewModel.prototype, "Id", {
            get: function () {
                return this._id;
            },
            enumerable: true,
            configurable: true
        });

        DiagramViewModel.prototype.addNewPoint = function (linesPoint) {
            //console.log("newPoint");
            //получаем идентификатор линию в которую будем вставлять точки.
            var lineId = linesPoint.LineId;

            // линия, вместе с точками
            var lineWithPoints;

            for (var i = 0; i < this.chartLinesCollection.length; i++) {
                if (this.chartLinesCollection[i].Id == lineId) {
                    lineWithPoints = this.chartLinesCollection[i];
                    break;
                }
            }

            //не нашли, ну и ладно.
            if (lineWithPoints == undefined) {
                console.log("lineWithPoints Not Find");
                return;
            }

            //разбираем пришедшее значение по оси из строки в число
            var time = Date.parse(linesPoint.X);
            var dateTime = new Date(time);
            var point = new Main.Point(dateTime, linesPoint.Y);

            //console.log(point);
            //добавляем точку x/y в коллекцию.
            lineWithPoints.Points.push(point);

            //console.log("Count=" + lineWithPoints.Points.length);
            //console.log("DiagramType: " + this._DiagramType);
            //console.log("FirstPart: "+(this._DiagramType == DiagramType.Oscilloscope));
            //console.log("SecondPart: " + (maxElementForOscilloscope < lineWithPoints.Points.length));
            if ((this._DiagramType == 0 /* Oscilloscope */) && (this.maxElementForOscilloscope < lineWithPoints.Points.length)) {
                while (lineWithPoints.Points.length > this.maxElementForOscilloscope) {
                    //console.log("In Loop");
                    //console.log(lineWithPoints.Points.length);
                    lineWithPoints.Points.shift();
                }
            }
        };

        DiagramViewModel.prototype.render = function () {
            this.chart.render();
        };

        DiagramViewModel.prototype.close = function () {
            //удаляем из dom
            $("#" + this._id).remove(".chartContainer");
            $("#buttonDiv" + this._id).remove(".bottonContainer");
        };

        DiagramViewModel.prototype.changeDiagramType = function () {
            //console.log(this._DiagramType);
            if (this._DiagramType == 0 /* Oscilloscope */) {
                this._DiagramType = 1 /* Seismograph */;
                $('#changeButton' + this.Id).prop('value', 'Seismograph');
            } else {
                this._DiagramType = 0 /* Oscilloscope */;
                $('#changeButton' + this.Id).prop('value', 'Oscilloscope');
            }
        };

        DiagramViewModel.prototype.onChangeDataCollectingClick = function () {
            var val = $('#stopButton' + this.Id).prop('value');
            if (val == 'Stop') {
                $('#stopButton' + this.Id).prop('value', 'Start');
                return false;
            } else {
                $('#stopButton' + this.Id).prop('value', 'Stop');
                return true;
            }
        };
        return DiagramViewModel;
    })();
    Main.DiagramViewModel = DiagramViewModel;
})(Main || (Main = {}));
//# sourceMappingURL=DiagramViewModel.js.map
