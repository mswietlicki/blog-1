module Main {

    export class DiagramPointModel {

        private _Y: number;
        get Y(): number {
            return this._Y;
        }
        set Y(id: number) {
            this._Y = id;
        }


        private _X: string;
        get X(): string {
            return this._X;
        }
        set X(id: string) {
            this._X = id;
        }


        private _LineId: string;
        get LineId(): string {
            return this._LineId;
        }
        set LineId(id: string) {
            this._LineId = id;
        }


        private _DiagramId: string;
        get DiagramId(): string {
            return this.DiagramId;
        }
        set DiagramId(id: string) {
            this._DiagramId = id;
        }
    }
}  