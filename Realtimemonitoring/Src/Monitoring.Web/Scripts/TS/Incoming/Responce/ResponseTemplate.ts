module Main {
    export class ResponceTemplate {
        constructor() {

        }

        private _success: boolean;

        get Success(): boolean {
            return this._success;
        }
        set Success(success: boolean) {
            this._success = success;
        }


        private _message: string;

        get Message(): string {
            return this._message;
        }
        set Message(message: string) {
            this._message = message;
        }
    }
} 