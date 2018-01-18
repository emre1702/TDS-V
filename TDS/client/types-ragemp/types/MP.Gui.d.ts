/// <reference path="../index.d.ts" />

declare interface MpGui {
    readonly cursor: MpGuiCursor;
    readonly chat: MpGuiChat;
    execute( code: string ): void;
}