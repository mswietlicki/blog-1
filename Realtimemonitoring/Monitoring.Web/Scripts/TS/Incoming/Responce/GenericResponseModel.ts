/// <reference path="ResponseTemplate.ts" />
module Main {

    export class GenericResponseModel<T> extends ResponceTemplate {

        constructor() {
            super();
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