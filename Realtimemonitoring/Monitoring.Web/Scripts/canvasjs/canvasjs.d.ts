interface Chart {
    new (htmlTagId: string, properties: any)
    render();
}

interface CanvasJSStatic {
    Chart: Chart;
}

declare module "CanvasJS" {
    export = CanvasJS;
}
declare var CanvasJS: CanvasJSStatic;