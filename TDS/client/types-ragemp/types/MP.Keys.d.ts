/// <reference path="../index.d.ts" />

declare interface MpKeys {
	isUp ( keyCode: number ): void;
	isDown ( keyCode: number ): void;
	bind ( keyCode: number, toggle: boolean, handler: Function ): void;
}
