module Main {

    export class DiagramLineModel {

        private _id: string;

        get Id(): string {
            return this._id;
        }
        set Id(id: string) {
            this._id = id;
        }


        private _name: string;

        get Name(): string {
            return this._name;
        }
        set Name(name: string) {
            this._name = name;
        }
    }
}  