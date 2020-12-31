import { Directive, HostListener, Input } from '@angular/core';

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: '[closeOnDoubleClick]',
})
export class ToolbarDirective {
    @Input() closeOnDoubleClick: any;
    private origDisplay: string;
    private touchtime = 0;

    @HostListener('click', ['$event'])
    onDoubleClick(e: MouseEvent) {
        e.preventDefault();
        e.stopPropagation();
        if (this.touchtime == 0) {
            // set first click
            this.touchtime = new Date().getTime();
        } else {
            // compare first click to this click and see if they occurred within double click threshold
            if (new Date().getTime() - this.touchtime < 800) {
                // double click occurred
                this.doubleClicked();
                this.touchtime = 0;
            } else {
                // not a double click so set as a new first click
                this.touchtime = new Date().getTime();
            }
        }
    }

    private doubleClicked() {
        if (!this.closeOnDoubleClick) {
            return;
        }
        if (this.closeOnDoubleClick.style.display !== 'none') {
            this.origDisplay = this.closeOnDoubleClick.style.display;
            this.closeOnDoubleClick.style.display = 'none';
        } else {
            this.closeOnDoubleClick.style.display = this.origDisplay;
        }
    }
}
