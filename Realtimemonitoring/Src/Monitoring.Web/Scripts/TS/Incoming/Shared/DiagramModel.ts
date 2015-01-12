/// <reference path="diagramtype.ts" />

module Main {

    export class DiagramModel {

        constructor() {
           
        }

        private _name: string;

        get Name(): string {
            return this._name;
        }
        set Name(name: string) {
            this._name = name;
        }


        private _id: string;

        get Id(): string {
            return this._id;
        }
        set Id(id: string) {
            this._id = id;
        }

        private _lines: DiagramLineModel[];

        get Lines(): DiagramLineModel[] {
            return this._lines;
        }
        set Lines(lines: DiagramLineModel[]) {
            this._lines = lines;
        }

        private _diagramType: DiagramType;

        get DiagramType(): DiagramType {
            return this._diagramType;
        }
        set DiagramType(diagramType: DiagramType) {
            this._diagramType = diagramType;
        }
    }
}  