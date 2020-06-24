import { Component, Output, EventEmitter, TemplateRef, Input, AfterContentChecked, ViewChild, ElementRef, HostListener, ChangeDetectorRef } from '@angular/core';
import { isInputOrTextAreaElement, getContentEditableCaretCoords } from './mentionUtils';
import { getCaretCoordinates } from './caretCoords';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'mention-list',
    styleUrls: ['./mentionListComponent.scss'],
    template: `
        <ng-template #defaultItemTemplate let-item="item" let-isActive="isActive">
        {{ (isActive ? selectedInfoSelecter(item) : infoSelecter(item)) }}
        </ng-template>
        <div #list *ngIf="!hidden" [class.mention-dropdown]="dropUp"
        class="mat-app-background mat-elevation-z24 dropdown-menu scrollable-menu mention-menu">
           <button mat-button *ngFor="let item of items; let i = index"
           [class.mention-active]="activeIndex == i"
           (click)="activeIndex=i; itemClick.emit()"
           (mouseenter)="activeIndex=i">
            <ng-template [ngTemplateOutlet]="itemTemplate" [ngTemplateOutletContext]="{'item':item, 'isActive': activeIndex == i}"></ng-template>
            </button>
        </div>`

})

/*
<ul #list [hidden]="hidden" class="dropdown-menu scrollable-menu"
        [class.mention-menu]="!styleOff" [class.mention-dropdown]="!styleOff && dropUp">
            <li *ngFor="let item of items; let i = index"
                [class.active]="activeIndex==i" [class.mention-active]="!styleOff && activeIndex==i">
                <a class="dropdown-item" [class.mention-item]="!styleOff"
                (mousedown)="activeIndex=i;itemClick.emit();$event.preventDefault()">
                    <ng-template [ngTemplateOutlet]="itemTemplate" [ngTemplateOutletContext]="{'item':item}"></ng-template>
                </a>
            </li>
        </ul>*/

export class MentionListComponent implements AfterContentChecked {

    @Input() itemTemplate: TemplateRef<any>;
    @Output() itemClick = new EventEmitter();
    @ViewChild('list') list: ElementRef;
    @ViewChild('defaultItemTemplate') defaultItemTemplate: TemplateRef<any>;
    items: string[] = [];
    activeIndex = 0;
    hidden = false;
    styleOff = false;
    dropUp = false;
    infoSelecter: (item: any) => string;
    selectedInfoSelecter: (info: any) => string;
    private offset = 0;
    private coords: {top: number, left: number} = {top: 0, left: 0};

    constructor(private element: ElementRef, private changeDetector: ChangeDetectorRef) {}

    ngAfterContentChecked() {
        if (!this.itemTemplate) {
            this.itemTemplate = this.defaultItemTemplate;
        }
    }

    // lots of confusion here between relative coordinates and containers
    position(nativeParentElement: HTMLInputElement, iframe: HTMLIFrameElement = null) {
        if (isInputOrTextAreaElement(nativeParentElement)) {
            // parent elements need to have postition:relative for this to work correctly?
            this.coords = getCaretCoordinates(nativeParentElement, nativeParentElement.selectionStart, null);
            this.coords.top = nativeParentElement.offsetTop + nativeParentElement.offsetHeight + this.coords.top - nativeParentElement.scrollTop;
            this.coords.left = nativeParentElement.offsetLeft + this.coords.left - nativeParentElement.scrollLeft;
            // getCretCoordinates() for text/input elements needs an additional offset to position the list correctly
            this.offset = this.getBlockCursorDimensions(nativeParentElement).height;
        } else if (iframe) {
            const context: { iframe: HTMLIFrameElement, parent: Element } = { iframe, parent: iframe.offsetParent };
            this.coords = getContentEditableCaretCoords(context);
        } else {
            const doc = document.documentElement;
            const scrollLeft = (window.pageXOffset || doc.scrollLeft) - (doc.clientLeft || 0);
            const scrollTop = (window.pageYOffset || doc.scrollTop) - (doc.clientTop || 0);
            // bounding rectangles are relative to view, offsets are relative to container?
            const caretRelativeToView = getContentEditableCaretCoords({ iframe });
            const parentRelativeToContainer: ClientRect = nativeParentElement.getBoundingClientRect();
            this.coords.top = caretRelativeToView.top - parentRelativeToContainer.top + nativeParentElement.offsetTop - scrollTop;
            this.coords.left = caretRelativeToView.left - parentRelativeToContainer.left + nativeParentElement.offsetLeft - scrollLeft;
        }
        // set the default/inital position
        this.positionElement();
        this.changeDetector.detectChanges();
    }

    setHidden(hidden: boolean) {
        this.hidden = hidden;
        this.changeDetector.detectChanges();
    }

    detectChanges() {
        this.changeDetector.detectChanges();
    }

    get activeItem() {
        return this.items[this.activeIndex];
    }

    activateNextItem() {
        // adjust scrollable-menu offset if the next item is out of view
        const listEl: HTMLElement = this.list.nativeElement;
        const activeEl = listEl.getElementsByClassName('mention-active').item(0);
        if (activeEl) {
            const nextLiEl: HTMLElement = activeEl.nextSibling as HTMLElement;
            if (nextLiEl && nextLiEl.nodeName == "BUTTON") {
                const nextLiRect: ClientRect = nextLiEl.getBoundingClientRect();
                if (nextLiRect.bottom > listEl.getBoundingClientRect().bottom) {
                    listEl.scrollTop = nextLiEl.offsetTop + nextLiRect.height - listEl.clientHeight;
                }
            }
        }
        // select the next item
        this.activeIndex = Math.max(Math.min(this.activeIndex + 1, this.items.length - 1), 0);
        this.changeDetector.detectChanges();
    }

    activatePreviousItem() {
        // adjust the scrollable-menu offset if the previous item is out of view
        const listEl: HTMLElement = this.list.nativeElement;
        const activeEl = listEl.getElementsByClassName('mention-active').item(0);
        if (activeEl) {
            const prevLiEl: HTMLElement = activeEl.previousSibling as HTMLElement;
            if (prevLiEl && prevLiEl.nodeName == "BUTTON") {
                const prevLiRect: ClientRect = prevLiEl.getBoundingClientRect();
                if (prevLiRect.top < listEl.getBoundingClientRect().top) {
                    listEl.scrollTop = prevLiEl.offsetTop;
                }
            }
        }
        // select the previous item
        this.activeIndex = Math.max(Math.min(this.activeIndex - 1, this.items.length - 1), 0);
        this.changeDetector.detectChanges();
    }

    // reset for a new mention search
    reset() {
        this.list.nativeElement.scrollTop = 0;
        this.checkBounds();
        this.changeDetector.detectChanges();
    }

    @HostListener("window:keydown", ["$event"])
    keydownHandler(event: any) {
        if (this.hidden) {
            return;
        }
        if (event.key === "ArrowUp") {
            this.activatePreviousItem();
        } else if (event.key === "ArrowDown") {
            this.activateNextItem();
        }
    }

    // final positioning is done after the list is shown (and the height and width are known)
    // ensure it's in the page bounds
    private checkBounds() {
        let left = this.coords.left;
        const top = this.coords.top;
        let dropUp = this.dropUp;
        const bounds: ClientRect = this.list.nativeElement.getBoundingClientRect();
        // if off right of page, align right
        if (bounds.left + bounds.width > window.innerWidth) {
            left -= bounds.left + bounds.width - window.innerWidth + 10;
        }
        // if more than half off the bottom of the page, force dropUp
        // if ((bounds.top+bounds.height/2)>window.innerHeight) {
        //   dropUp = true;
        // }
        // if top is off page, disable dropUp
        if (bounds.top < 0) {
            dropUp = false;
        }
        // set the revised/final position
        this.positionElement(left, top, dropUp);
    }

    private positionElement(left: number = this.coords.left, top: number = this.coords.top, dropUp: boolean = this.dropUp) {
        const el: HTMLElement = this.element.nativeElement;
        top += true ? 0 : this.offset; // top of list is next line
        el.className = dropUp ? 'dropup' : null;
        el.style.position = "absolute";
        el.style.left = left + 'px';
        el.style.top = top + 'px';
        this.changeDetector.detectChanges();
    }

    private getBlockCursorDimensions(nativeParentElement: HTMLInputElement) {
        const parentStyles = window.getComputedStyle(nativeParentElement);
        return {
            height: parseFloat(parentStyles.lineHeight),
            width: parseFloat(parentStyles.fontSize)
        };
    }
}
