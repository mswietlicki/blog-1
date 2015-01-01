/// <reference path="../../signalr/signalr.d.ts" />
/// <reference path="../../jquery/jquery.d.ts" />
module Main {

    export class BasePageModel {

        //список диаграмм
        public _diagrams: Array<DiagramViewModel>;
        //SignalR Hub Proxy 
        public _diagramHub: HubProxy;
        //SignalR connection
        public _hubConnection: SignalR;


        constructor() {
            this._diagrams = new Array<DiagramViewModel>();
        }

        //действие на получение списка диаграмм
        public onGetDiagrams(data: GenericResponseModel<DiagramModel>) {
            //получив список диаграмм, для каждой из них выполняем подключение к соответсвующей группе в hub.
            data.Data.forEach(dataElement=> {
                this.joinGroup(dataElement);
            });
        }

        //действие для каждой диаграммы- подключение к Group.
        private joinGroup(diagram: DiagramModel) {

            //создаем визуальное представление диаграммы.
            var dl: DiagramViewModel = new DiagramViewModel(diagram);

            //сохраняем его себе.
            this._diagrams.push(dl);

            //получаем исторические данные, за последние сколько-то минут, до того как начинаем получать push нотификации.
            $.get(("/api/Diagram/GetDiagramData/" + diagram.Id))
                .done(data=> {
                    var localData: GenericPushModel<UpdateDiagramModel> = <GenericPushModel<UpdateDiagramModel>>data;

                    this.onUpdateRecieve(localData);
                });

            //вызываем метод hub подключиться к группе, чтобы получать нотификации.
            this._diagramHub.server.joinGroup(diagram.Id).done(() => {
                //console.log("JoinGroup");
            });
        }

        public renderHeader(data: GenericResponseModel<DiagramModel>) {
            $("body").append("<header class='headerContainer'>" + "</header>");
            $("header").append("<a href=\"/home/index/\">Home</a>");
            data.Data.forEach(dataElement=> {
                $("header").append("<a href=\"/home/chart/" + dataElement.Id + "\">" + dataElement.Name + "</a>");
            });
        }

        //пришло с signalr hub сообщение, что появились новые точки для отрисовки.
        //или мы получили исторические данные из controller.
        public onUpdateRecieve(data: GenericPushModel<UpdateDiagramModel>) {
            //console.log("newData");

            //добавляем каждую точку к соответсвующей диаграмме
            data.Data.forEach(diagram=> {
                var diagramId = diagram.DiagramId;

                var localDiagram: DiagramViewModel;

                //ищем диаграмму
                for (var i = 0; i < this._diagrams.length; i++) {
                    if (this._diagrams[i].Id == diagramId) {
                        localDiagram = this._diagrams[i];
                        break;
                    }
                }
                if (localDiagram == undefined) {
                    console.log("Diagram Not Find");
                    return;
                }

                //добавляем все точки, которые относятся к этой диаграмме.
                diagram.Points.forEach(point=> {
                    localDiagram.addNewPoint(point);
                });

                //рендерим диаграмму заново.
                localDiagram.render();
            });
        }

        public close(diagramId: string) {
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
        }

        public changeDiagramType(diagramId: string) {
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

        }

        public onChangeDataCollectingClick(diagramId: string) {

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
                this._diagramHub.server.leftGroup(diagramId).done(() => {
                    //console.log("leftGroup");
                });
            } else {
                //вызываем метод hub переподключения к группе, чтобы получать нотификации.
                this._diagramHub.server.joinGroup(diagramId).done(() => {
                    //console.log("joinGroup");
                });
            }
        }

        private getDiagramAndIndex(diagramId: string) {

            var diagramIndex = -1;
            var localDiagram = null;

            //ищем диаграмму
            for (var i = 0; i < this._diagrams.length; i++) {
                if (this._diagrams[i].Id == diagramId) {
                    diagramIndex = i;
                    localDiagram = this._diagrams[i];
                    break;
                }
            }
            return { localDiagram: localDiagram, diagramIndex: diagramIndex };
        }
    }
}