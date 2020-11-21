import { Directive, OnDestroy, ChangeDetectorRef, ViewContainerRef, AfterViewInit } from '@angular/core';
import { ApplyBackgroundService } from '../services/apply-background.service';

@Directive({
  selector: ".mat-app-background",
})
export class MatAppBackgroundDirective implements AfterViewInit, OnDestroy {

    constructor(
        private changeDetector: ChangeDetectorRef,
        private viewContainerRef: ViewContainerRef,
        private applyBackgroundService: ApplyBackgroundService
    ) {
        applyBackgroundService.changed.on(null, () => this.changeDetector.detectChanges());
    }

    ngAfterViewInit() {
        this.applyBackgroundService.add(this.viewContainerRef.element.nativeElement);
        this.changeDetector.detectChanges();
    }

    ngOnDestroy() {
        this.applyBackgroundService.remove(this.viewContainerRef.element.nativeElement);
    }
}
