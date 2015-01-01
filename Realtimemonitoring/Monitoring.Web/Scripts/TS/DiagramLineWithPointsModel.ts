/// <reference path="Point.ts" />
/// <reference path="Incoming/Shared/DiagramLineModel.ts" />
/// <reference path="Incoming/Shared/DiagramPointModel.ts" />

module Main {
    //линия с точками
    export class DiagramLineWithPointsModel extends DiagramLineModel {

        //массиво точек для отображения
        private _points: Array<Point>;
             
        get Points(): Array<Point> {
            return this._points;
        }

        constructor(id: string, name: string) {
            super();
            this.Id = id;
            this.Name = name;
            this._points = new Array<Point>();
        }
    }
}