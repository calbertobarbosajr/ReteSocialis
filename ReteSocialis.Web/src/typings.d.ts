declare module 'cropperjs';
// src/typings.d.ts
declare module 'cropperjs' {
  export default class Cropper {
    constructor(element: HTMLImageElement, options?: any);
    destroy(): void;
    getCroppedCanvas(options?: any): HTMLCanvasElement;
  }
}
