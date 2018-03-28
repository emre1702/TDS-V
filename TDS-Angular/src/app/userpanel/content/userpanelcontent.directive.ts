import { Directive, ViewContainerRef } from "@angular/core";

@Directive({
  selector: "[appUserpanelContent]",
})

export class UserpanelContentDirective {
  constructor(public viewContainerRef: ViewContainerRef) {}
}