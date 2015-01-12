module Main {

    export class GenericPushModel<T> {

        constructor() {

        }

        private _data: Array<T>;

        get Data(): Array<T> {
            return this._data;
        }
        set Data(data: Array<T>) {
            this._data = data;
        }
    }
}  