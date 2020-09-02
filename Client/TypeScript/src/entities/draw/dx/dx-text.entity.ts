import DxType from "../../../datas/enums/draw/dx-type.enum";
import game from "natives";
import alt from "alt-client";
import Font from "../../../datas/enums/draw/font.enum";
import DxService from "../../../services/draw/dx.service";
import Alignment from "../../../datas/enums/draw/alignment.enum";
import DxBase from "./base/dx-base.entity";

export class DxText extends DxBase {

    private leftBoundary: number = 0;
    private rightBoundary: number = 1;
    private textBlocks: string[];

    private scaleAnim: { targetScale: number, startMs: number, endMs: number };

    constructor(dxService: DxService, text: string, private x: number, private y: number, private scale: number, private color: alt.RGBA, private font: Font = Font.ChaletLondon,
                private alignmentX: Alignment = Alignment.Center, private alignmentY: Alignment = Alignment.Center,
                private relative: boolean = true, private dropShadow: boolean = false, private outline: boolean = false,
                frontPriority: number = 0, activated: boolean = true) {
        super(dxService, frontPriority, activated);

        this.setText(text);
        this.setColor(color);

        if (!relative) {
            this.x = this.getRelativeX(this.x, false);
            this.y = this.getRelativeY(this.y, false);
        }
    }

    setText(text: string) {
        this.textBlocks.length = 0;
        text.match(/.{1,99}/g).forEach(textBlock => this.textBlocks.push(textBlock));
    }

    setColor(color: alt.RGBA) {
        this.color = color;
        this.alphaAnim = undefined;
    }

    setScale(scale: number) {
        this.scale = scale;
        this.scaleAnim = undefined;
    }

    setY(y: number, relative: boolean) {
        this.y = this.getRelativeY(y, relative);
        this.y += this.getAddingToYForAlignment();
    }

    setBoundary(left: number, right: number, relative: boolean = this.relative) {
        this.leftBoundary = this.getRelativeX(left, relative);
        this.rightBoundary = this.getRelativeX(right, relative);
    }

    blendScale(endScale: number, msToEnd: number) {
        const currentMs = Date.now();
        this.scaleAnim = { targetScale: endScale, startMs: currentMs, endMs: currentMs + msToEnd };
    }

    draw(): void {
        const alpha = this.getCurrentAlpha(this.color.a);
        const scale = this.getCurrentScale();

        game.setTextFont(this.font);
        game.setTextProportional(false);
        game.setTextScale(scale, scale);
        game.setTextColour(this.color.r, this.color.g, this.color.b, alpha);
        game.setTextEdge(2, 0, 0, 0, 150);
        if (this.dropShadow) {
            game.setTextDropshadow(0, 0, 0, 0, 255);
            game.setTextDropShadow();
        }
        if (this.outline) {
            game.setTextOutline();
        }
        game.setTextWrap(this.leftBoundary, this.rightBoundary);

        //game.setTextWrap(0, 1);
        game.setTextCentre(true);

        game.setTextJustification(this.alignmentX);

        game.beginTextCommandDisplayText("CELL_EMAIL_BCON");
        //Split text into pieces of max 99 chars blocks
        for (const text of this.textBlocks) {
            game.addTextComponentSubstringPlayerName(text);
        }
        game.endTextCommandDisplayText(this.x, this.y, 0.0);
    }

    getDxType(): DxType {
        return DxType.Text;
    }

    private getCurrentScale(): number {
        if (!this.scaleAnim) {
            return this.scale;
        }

        return this.getBlendValue(this.scale, this.scaleAnim.targetScale, this.scaleAnim.startMs, this.scaleAnim.endMs);
    }

    //Todo Implement this
    private getAddingToYForAlignment() {
        switch (this.alignmentY) {
            case Alignment.Start:
                return 0;
        }
        return 0;
    }
}
