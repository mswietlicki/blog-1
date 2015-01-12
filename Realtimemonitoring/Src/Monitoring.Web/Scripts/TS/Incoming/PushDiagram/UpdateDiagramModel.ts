module Main {
    export class UpdateDiagramModel {

        private _diagramId: string;

        get DiagramId(): string {
            return this._diagramId;
        }
        set DiagramId(name: string) {
            this._diagramId = name;
        }


        private _points: Array<DiagramPointModel>;

        get Points(): Array<DiagramPointModel> {
            return this._points;
        }
        set Points(diagrams: Array<DiagramPointModel>) {
            this._points = diagrams;
        }

        
        constructor() {
            this._points = new Array<DiagramPointModel>();
        }
    }
}  