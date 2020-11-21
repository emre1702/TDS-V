/**
 * This is an old code I wanted to test.
 * With this I could use custom background for mat menu.
 * If I wanted to implement this, I'd have to use a service on @root instead.
 * Directive is wrong because it's not attached to .mat-menu, it's attached to the body.
 * The body gets a cdk-overlay-container (on menu open) where the panels are added.
 * Also it worked only once for menu panel on opening - would need to make it decently.
 */


/*import { Directive, ViewContainerRef, ChangeDetectorRef, OnDestroy, AfterViewInit } from '@angular/core';
import { ApplyBackgroundService } from '../services/apply-background.service';

@Directive({
    selector: ".mat-menu",
})
export class MatMenuDirective implements AfterViewInit, OnDestroy {
    private addedPanelIds: number[] = [];

    constructor(
        private changeDetector: ChangeDetectorRef,
        private viewContainerRef: ViewContainerRef,
        
        private applyBackgroundService: ApplyBackgroundService
    ) {
        applyBackgroundService.changed.on(null, () => this.changeDetector.detectChanges());

        const observer = new MutationObserver((records: MutationRecord[]) => {
            console.log(records);
            for (const { addedNodes } of records) {
                const container = this.findAddedCdkOverlayContainer(addedNodes);
                if (!container) return;

                console.log(container);
                const matMenuPanels = this.findAddedMatMenuPanels(container);
                if (matMenuPanels.length != 0) {
                    this.handleAddedMatMenuPanels(matMenuPanels);
                }

                console.log(matMenuPanels);
                const observer = new MutationObserver((observers) => {
                    console.log(observers);
                });
                observer.observe(container, {
                    childList: true
                });
            }
        });
        observer.observe(document.body, {
            childList: true
        });
    }

    ngAfterViewInit() {
        console.log(this.viewContainerRef);
        this.changeDetector.detectChanges();
    }

    ngOnDestroy() {

    }

    private findAddedCdkOverlayContainer(addedNodes: any): any | undefined {
        for (const addedNode of addedNodes) {
            if (addedNode.className === "cdk-overlay-container") 
                return addedNode;
        }

        return undefined;
    }

    private findAddedMatMenuPanels(container: any): [] {
        return container.getElementsByClassName("mat-menu-panel");
    }

    private handleAddedMatMenuPanels(panels: []) {
        for (const panel of panels) {
            const id = Number(((panel as any).id as string).substr("mat-menu-panel-".length));
            if (isNaN(id) || this.addedPanelIds.indexOf(id) > 0) {
                continue;
            }
            this.addedPanelIds.push(id);
            console.log(panel);
            this.applyBackgroundService.add(panel);
        }
        
    }
}*/