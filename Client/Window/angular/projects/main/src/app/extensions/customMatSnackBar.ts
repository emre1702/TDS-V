import { Component, Inject, OnInit } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'custom-mat-snack-bar',
    styles: [
        ".mat-app-background { white-space: pre-line; width: 100%; height: 100%; display: flex; flex-direction: row; padding: 0 16px 0 16px; }",
        ".text { margin: 14px auto 14px auto; }",
        ".mat-icon-button { margin-top: auto; margin-bottom: auto; }",
        ".placeholder { width: 10px; }"
    ],
    template: `
<div class="mat-app-background">
    <span class="text">{{ data }}</span>
    <div class="placeholder"></div>
    <button mat-icon-button (click)="snackBarRef.dismiss()">
        <mat-icon>close</mat-icon>
    </button>
</div>
  `
})
export class CustomMatSnackBarComponent {

    constructor(
        public snackBarRef: MatSnackBarRef<CustomMatSnackBarComponent>,
        @Inject(MAT_SNACK_BAR_DATA) public data: any) { }
}
