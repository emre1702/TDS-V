import { Component, ComponentFactoryResolver, OnInit, ViewChild, ChangeDetectorRef, ElementRef } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";

import { UserpanelContentDirective } from "./content/userpanelcontent.directive";
import { UserpanelContentComponent } from "./content/userpanelcontent.component";
import { UserpanelContentService } from "./content/userpanelcontent.service";
import { UserpanelContentItem } from "./content/userpanelcontent.item";
import { RAGE } from "../rageconnector/rageconnector.service";
import { MatSidenav } from "@angular/material";
import { EventEmitter } from "protractor";

@Component({
  selector: "app-userpanel",
  templateUrl: "./userpanel.component.html",
  styleUrls: ["./userpanel.component.css"]
})

export class UserpanelComponent implements OnInit {
    opened = false;
    private menus = [];
    private openedMenu = this.menus[0];
    language;
    private components: UserpanelContentItem[];
    @ViewChild(UserpanelContentDirective) userpanelContentDirective: UserpanelContentDirective;
    @ViewChild("sidenav") sidenav: MatSidenav;
    private currentOpenedComponent: UserpanelContentComponent;

    constructor ( 
        private componentFactoryResolver: ComponentFactoryResolver, 
        private userpanelContentService: UserpanelContentService, 
        private cdRef: ChangeDetectorRef, 
        private rage: RAGE ) {
            this.rage.Client.call ( {
                fn: "requestLanguage",
                args: []
            },
            ( response: ("ENGLISH"|"GERMAN") ) => {
                userpanelContentService.myLanguage = response;
                this.language = userpanelContentService.getLang( "main" );
                // this.sidenav.opened = true;
            } );
            rage.Client.listen( this, this.onOpen, "openUserpanel" );
            rage.Client.listen( this, this.onClose, "closeUserpanel" );
    }

    onOpen() {
        this.opened = true;
        this.cdRef.detectChanges();
        this.changeMenu ( 0 ); 
    }

    onClose() {
        this.opened = false;
        this.cdRef.detectChanges();
    }
    
    ngOnInit() {
        this.loadAllComponents();
    }

    changeMenu ( index: number ) {
        if ( index < this.components.length ) {
            this.openedMenu = this.menus[index]; 
            this.loadComponent ( index );
        }
    }

    close() {
        this.rage.Client.call ( {
            fn: "closeUserpanel",
            args: []
        } );
        this.onClose();
    }

    loadComponent ( index: number ) {
        let component: UserpanelContentItem = this.components[index];
    
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(component.component);

        let viewContainerRef = this.userpanelContentDirective.viewContainerRef;
        viewContainerRef.clear();
    
        let componentRef = viewContainerRef.createComponent(componentFactory);

        if ( this.currentOpenedComponent )
            this.currentOpenedComponent.onClose.bind(this.currentOpenedComponent);
        this.currentOpenedComponent = (<UserpanelContentComponent>componentRef.instance); 
        this.currentOpenedComponent.onOpen.bind(this.currentOpenedComponent);
        this.cdRef.detectChanges(); 
    }

    loadAllComponents() {
        this.menus = this.userpanelContentService.getMenuNames();
        this.components = this.userpanelContentService.getContents();
        this.cdRef.detectChanges();
    }
}

