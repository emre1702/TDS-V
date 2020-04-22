import { Directive,  HostListener, Input, ComponentRef } from '@angular/core';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: "mat-toolbar[closeOnDoubleClick]"
})
export class ToolbarDirective {

    @Input() closeOnDoubleClick: any;
    private origDisplay: string;

    constructor() {}

    @HostListener('dblclick', ['$event'])
    onDoubleClick(e: MouseEvent) {
        e.preventDefault();
        if (this.closeOnDoubleClick.style.display !== "none") {
            this.origDisplay = this.closeOnDoubleClick.style.display;
            this.closeOnDoubleClick.style.display = "none";
        } else {
            this.closeOnDoubleClick.style.display = this.origDisplay;
        }
    }

}
