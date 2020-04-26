import { HostListener, Input, ElementRef, ViewContainerRef, Component, Directive, ComponentFactoryResolver, TemplateRef, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { MentionListComponent } from './mentionListComponent';
import { MentionConfig } from './mentionConfig';


@Directive({
    // tslint:disable-next-line: directive-selector
    selector: "[mention]"
})
export class MentionDirective {

    @Input() mention: MentionConfig;
    @Input() mentionListTemplate: TemplateRef<any>;
    @Output() mentionShowingChanged: EventEmitter<boolean> = new EventEmitter<boolean>();

    private searchList: MentionListComponent;
    private searchString: string;

    constructor(
        private _element: ElementRef,
        private _componentResolver: ComponentFactoryResolver,
        private _viewContainerRef: ViewContainerRef,
        private changeDetector: ChangeDetectorRef) { }


    @HostListener("input", ["$event"])
    inputHandler(event: any, nativeElement: HTMLInputElement = this._element.nativeElement) {

        if (event.data === this.mention.triggerChar
            || this.mention.triggerChar == "@" && event.key == "q" && event.ctrlKey && event.altKey)
            return;
        if (this.searchString !== undefined) {
            if (event.data) {
                this.searchString += event.data;
            } else {
                this.searchString = this.searchString.substr(0, this.searchString.length - 1);
            }

            this.updateSearchList();
        }
    }

    @HostListener("keydown", ["$event"])
    keyDownHandler(event: any, nativeElement: HTMLInputElement = this._element.nativeElement) {
        if (event.key === this.mention.triggerChar
            || this.mention.triggerChar == "@" && event.key == "q" && event.ctrlKey && event.altKey) {
            if (!this.searchList || this.searchList.hidden) {
                this.openSearchList(nativeElement);
            }
            this.updateSearchList();
        }
    }

    @HostListener("keyup", ["$event"])
    keyUpHandler(event: any) {
        if (this.searchString !== undefined) {
            if (event.key === "Enter") {
                this.selectFromSearchList();
            } else if (event.key === " ") {
                this.closeSearchList();
            } else if (event.key === "Backspace" || event.key === "Delete") {
                this.reloadSearchString();
            }
        }
    }

    private openSearchList(nativeElement: HTMLInputElement) {
        if (!this.searchList) {
            const componentFactory = this._componentResolver.resolveComponentFactory(MentionListComponent);
            const componentRef = this._viewContainerRef.createComponent(componentFactory);
            this.searchList = componentRef.instance;
            this.searchList.itemTemplate = this.mentionListTemplate;
            componentRef.instance.itemClick.subscribe(() => {
                nativeElement.focus();
                const fakeKeydown = { key: "Enter", wasClick: true };
                this.keyUpHandler(fakeKeydown);
            });
        }
        // this.searchList.dropUp = this.mention.dropUp;
        // this.searchList.styleOff = this.mention.disableStyle;
        this.searchList.activeIndex = 0;
        this.searchList.dropUp = this.mention.dropUp || false;
        this.searchList.position(nativeElement);
        this.searchString = "";
        window.setTimeout(() => this.searchList.reset());
        this.changeDetector.detectChanges();
    }

    private selectFromSearchList() {
        if (!this.searchList.items.length) {
            return;
        }

        if (!this.searchString.length || this.searchString[this.searchString.length - 1] !== this.mention.seachStringEndChar) {
            const str = this.mention.mentionSelect(this.searchList.activeItem);
            let cursorPos = this.doGetCaretPosition(this._element.nativeElement);
            const currentValue = this._element.nativeElement.value as string;
            if (cursorPos === 0) {
                cursorPos = currentValue.length;
            }
            const startIndex = currentValue.lastIndexOf(this.mention.triggerChar, cursorPos);
            const spaceIndex = currentValue.indexOf(" ", startIndex);
            // Don't allow spaces in @mention
            if (spaceIndex < 0 || spaceIndex > cursorPos) {
                this._element.nativeElement.value = currentValue.substring(0, startIndex) + str + currentValue.substring(cursorPos);
            }
        }

        this.closeSearchList();
    }

    closeSearchList() {
        if (this.searchList) {
            this.searchList.setHidden(true);
            this.mentionShowingChanged.emit(false);
        }
        this.searchString = undefined;
        this.mentionShowingChanged.emit(false);

        this.changeDetector.detectChanges();
    }

    private reloadSearchString() {
        if (this.searchList.hidden && !this.searchString) {
            return;
        }

        const cursorPos = this.doGetCaretPosition(this._element.nativeElement);
        const currentValue = this._element.nativeElement.value as string;
        const startIndex = currentValue.lastIndexOf(this.mention.triggerChar, cursorPos);
        if (startIndex < 0) {
            this.closeSearchList();
            return;
        }
        const spaceIndex = currentValue.indexOf(" ", startIndex);
        if (spaceIndex < 0) {
            this.searchString = currentValue.substring(startIndex + 1);
        } else if (spaceIndex > cursorPos) {
            this.searchString = currentValue.substring(startIndex + 1, spaceIndex);
        } else {
            this.closeSearchList();
        }
    }

    private updateSearchList() {
        let matches: string[] = [];

        if (this.mention && this.mention.items) {
            let objects = this.mention.items;
            if (this.searchString) {
                const searchStringLowerCase = this.searchString.toLowerCase();
                objects = objects.filter(e => e.toLowerCase().indexOf(searchStringLowerCase) >= 0);
            }
            matches = objects;
            if (!this.mention.maxItems) {
                this.mention.maxItems = 99;
            }
            if (this.mention.maxItems > 0) {
                matches = matches.slice(0, this.mention.maxItems);
            }
        }

        if (this.searchList) {
            this.searchList.items = matches;
            this.searchList.setHidden(matches.length == 0);
            this.mentionShowingChanged.emit(!this.searchList.hidden);
            this.searchList.position(this._element.nativeElement);
        }
        this.changeDetector.detectChanges();
    }

    refreshItems(items: string[]) {
        if (this.searchList) {
            this.searchList.items = items;
            this.searchList.detectChanges();
        }
        this.mention.items = items;
        this.changeDetector.detectChanges();
    }

    private doGetCaretPosition(oField: HTMLInputElement) {
        // Initialize
        let iCaretPos = 0;
        // IE Support
        if ((document as any).selection) {
          // Set focus on the element
          oField.focus();
          // To get cursor position, get empty selection range
          const oSel = (document as any).selection.createRange();
          // Move selection start to 0 position
          oSel.moveStart('character', -oField.value.length);
          // The caret position is selection length
          iCaretPos = oSel.text.length;
        } else if (oField.selectionStart || oField.selectionStart == 0)
          iCaretPos = oField.selectionDirection == 'backward' ? oField.selectionStart : oField.selectionEnd;

        // Return results
        return iCaretPos;
      }
}
