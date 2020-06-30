import { HostListener, Input, ElementRef, ViewContainerRef, Directive, ComponentFactoryResolver,
    TemplateRef, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { MentionListComponent } from './mentionListComponent';
import { MentionConfig } from './mentionConfig';


@Directive({
    // tslint:disable-next-line: directive-selector
    selector: "[mention]"
})
export class MentionDirective {

    @Input() mention: MentionConfig[];
    @Input() mentionListTemplate: TemplateRef<any>;
    @Output() mentionShowingChanged: EventEmitter<boolean> = new EventEmitter<boolean>();

    private searchList: MentionListComponent;
    private searchString: string;
    private currentMentionIndex = -1;

    constructor(
        private _element: ElementRef,
        private _componentResolver: ComponentFactoryResolver,
        private _viewContainerRef: ViewContainerRef,
        private changeDetector: ChangeDetectorRef) { }


    @HostListener("input", ["$event"])
    inputHandler(event: any) {

        if (this.currentMentionIndex === -1) {
            return;
        }

        if (event.data === this.mention[this.currentMentionIndex].triggerChar
            || this.mention[this.currentMentionIndex].triggerChar == "@" && event.key == "q" && event.ctrlKey && event.altKey)
            return;

        if (event.data) {
            this.searchString += event.data;
        } else {
            this.searchString = this.searchString.substr(0, this.searchString.length - 1);
        }

        this.updateSearchList();
    }

    @HostListener("keydown", ["$event"])
    keyDownHandler(event: any, nativeElement: HTMLInputElement = this._element.nativeElement) {
        if (this.currentMentionIndex !== -1) {
            return;
        }

        for (const config of this.mention) {
            if ((event.key === config.triggerChar
                || config.triggerChar == "@" && event.key == "q" && event.ctrlKey && event.altKey
                || config.triggerChar == "/" && event.key == "7" && event.shiftKey)
                &&
                (!config.onlyAllowAtBeginning || !this._element.nativeElement.value.length)) {
                this.currentMentionIndex = this.mention.indexOf(config);
                if (!this.searchList || this.searchList.hidden) {
                    this.openSearchList(nativeElement);
                }
                this.updateSearchList();
                break;
            }
        }
    }

    @HostListener("keyup", ["$event"])
    keyUpHandler(event: any) {
        if (this.currentMentionIndex !== -1) {
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
        const currentConf = this.mention[this.currentMentionIndex];
        // this.searchList.dropUp = this.mention[this.currentMentionIndex].dropUp;
        // this.searchList.styleOff = this.mention[this.currentMentionIndex].disableStyle;
        this.searchList.activeIndex = 0;
        this.searchList.dropUp = currentConf.dropUp || false;
        this.searchList.position(nativeElement);
        this.searchList.infoSelecter = currentConf.mentionInfo;
        this.searchList.selectedInfoSelecter = currentConf.mentionSelectedInfo;
        this.searchString = "";
        window.setTimeout(() => this.searchList.reset());
        this.changeDetector.detectChanges();
    }

    private selectFromSearchList() {
        if (!this.searchList.items.length) {
            return;
        }

        if (!this.searchString.length || this.searchString[this.searchString.length - 1]
            !== this.mention[this.currentMentionIndex].seachStringEndChar) {
            const str = this.mention[this.currentMentionIndex].mentionSelect(this.searchList.activeItem);
            let cursorPos = this.doGetCaretPosition(this._element.nativeElement);
            const currentValue = this._element.nativeElement.value as string;
            if (cursorPos === 0) {
                cursorPos = currentValue.length;
            }
            const startIndex = currentValue.lastIndexOf(this.mention[this.currentMentionIndex].triggerChar, cursorPos);
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
        this.currentMentionIndex = -1;
        this.mentionShowingChanged.emit(false);

        this.changeDetector.detectChanges();
    }

    private reloadSearchString() {
        if (this.searchList.hidden && !this.searchString) {
            return;
        }

        const cursorPos = this.doGetCaretPosition(this._element.nativeElement);
        const currentValue = this._element.nativeElement.value as string;
        const startIndex = currentValue.lastIndexOf(this.mention[this.currentMentionIndex].triggerChar, cursorPos);
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

        if (this.mention && this.mention[this.currentMentionIndex].items) {
            const currentConf = this.mention[this.currentMentionIndex];
            let objects = currentConf.items;
            if (this.searchString) {
                const searchStringLowerCase = this.searchString.toLowerCase();
                objects = objects.filter(e => currentConf.mentionSearch(e, searchStringLowerCase));
            }
            matches = objects;
            if (!currentConf.maxItems) {
                currentConf.maxItems = 99;
            }
            if (currentConf.maxItems > 0) {
                matches = matches.slice(0, currentConf.maxItems);
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

    refreshItems(items: string[], configIndex: number) {
        if (this.searchList && this.currentMentionIndex == configIndex) {
            this.searchList.items = items;
            this.searchList.detectChanges();
        }
        this.mention[configIndex].items = items;
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
