import { Component, OnInit, Input, ViewChild, ChangeDetectorRef, AfterViewInit, ElementRef, HostListener } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { SettingsService } from '../../services/settings.service';
import { trigger, transition, query, style, stagger, animate, AnimationBuilder, AnimationPlayer } from '@angular/animations';
import { LobbyChoice } from '../lobbychoice/lobby-choice/interfaces/lobby-choice';
import { isNumber } from 'util';

export enum KEY_CODE {
    RIGHT_ARROW = 39,
    LEFT_ARROW = 37
}

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'carousel',
    animations: [
        trigger('hideShowAnimation', [
            transition('* => *', [
                query(':enter', [
                    style({ display: 'inline-block', opacity: 0 }),
                    stagger(1300, [
                        animate('500ms', style({ opacity: 0.8 })),
                    ])
                ], { optional: true }),
                query(':leave', [
                    style({ display: 'inline-block', opacity: 0.8 }),
                    stagger(1300, [
                        animate('500ms', style({ display: 'none', opacity: 0 }))
                    ])
                ], { optional: true })
            ])
        ]),
    ],
    templateUrl: './carousel.component.html',
    styleUrls: ['./carousel.component.scss']
})
export class CarouselComponent implements AfterViewInit {

    private buttonsWidth = 0.6;

    private theta: number;
    private angle = 0;
    private radius: number;
    private selectedIndex = 0;
    private turnAroundPlayer: AnimationPlayer;

    @ViewChild("carousel", { static: false }) carousel: ElementRef;

    @Input() buttons: LobbyChoice[];
    buttonStyles: { transform: string }[] = [{ transform: "" }];

    constructor(
        public settings: SettingsService,
        public sanitizer: DomSanitizer,
        private changeDetector: ChangeDetectorRef,
        private animationBuilder: AnimationBuilder) { }

    ngAfterViewInit() {
        // Need to do it twice because the first time it uses the window.innerWidth
        // in the second time the button width correctly
        // We need to call this changeCarousel once (for buttonStyle)
        // else we cant get the button width correctly
        this.changeCarousel();
        this.changeDetector.detectChanges();
        this.changeCarousel();
        this.changeDetector.detectChanges();
        this.createTurnAroundAnimation();
    }

    private changeCarousel() {
        const cellCount = this.buttons.length;
        this.theta = 360 / cellCount;
        const cellSize = this.carousel ? this.carousel.nativeElement.offsetWidth : window.innerWidth * 0.6;
        this.radius = Math.round((cellSize / 2) / Math.tan(Math.PI / cellCount));
        for (let i = 0; i < cellCount; i++) {
            let btnStyle = this.buttonStyles[i + 1];
            if (!btnStyle) {
                btnStyle = { transform: "" };
                this.buttonStyles[i + 1] = btnStyle;
            }
            const cellAngle = this.theta * i;
            btnStyle.transform = 'rotateY(' + cellAngle + 'deg) translateZ(' + this.radius + 'px)';
        }

        this.rotateCarousel();
    }

    private rotateCarousel() {
        const btnStyle = this.buttonStyles[0];
        btnStyle.transform = 'translateZ(' + -this.radius + 'px) rotateY(' + this.angle + 'deg)';
    }

    private createTurnAroundAnimation() {
        const anim = this.animationBuilder.build([
            style({ transform: 'translateZ(' + -this.radius + 'px) rotateY(-60deg)' }),
            animate(500 * this.buttons.length, style({ transform: 'translateZ(' + -this.radius + 'px) rotateY(-360deg)' }))
        ]);

        this.turnAroundPlayer = anim.create(this.carousel.nativeElement);
        this.turnAroundPlayer.play();

        this.turnAroundPlayer.onDone(this.stopTurnAroundAnimation.bind(this));
    }

    getImageUrl(url: string) {
        return this.sanitizer.bypassSecurityTrustStyle("url(" + url + ") no-repeat");
    }

    @HostListener('window:keyup', ['$event'])
    keyEvent(event: KeyboardEvent) {
        if (event.key === "ArrowRight") {
            this.toRight();
        } else if (event.key === "ArrowLeft") {
            this.toLeft();
        } else if (event.key === "Enter") {
            this.buttons[this.buttons.length - this.selectedIndex - 1].func();
        } else if (!isNaN(parseInt(event.key, 10))) {
            const index = parseInt(event.key, 10);
            if (this.buttons.length >= index) {
                this.buttons.find(i => i.index == index - 1).func();
            }
        } else {
            this.stopTurnAroundAnimation();
        }
    }

    toRight() {
        this.stopTurnAroundAnimation();
        this.removeButtonsActiveClass();
        this.selectedIndex = this.selectedIndex + 1 >= this.buttons.length ? 0 : this.selectedIndex + 1;
        this.angle -= this.theta;
        this.setButtonsActiveClass();
        this.rotateCarousel();
        this.changeDetector.detectChanges();
    }

    toLeft() {
        this.stopTurnAroundAnimation();
        this.removeButtonsActiveClass();
        this.selectedIndex = this.selectedIndex - 1 < 0 ? this.buttons.length - 1 : this.selectedIndex - 1;
        this.angle += this.theta;
        this.setButtonsActiveClass();
        this.rotateCarousel();
        this.changeDetector.detectChanges();
    }

    private stopTurnAroundAnimation() {
        if (this.turnAroundPlayer) {
            this.turnAroundPlayer.destroy();
            this.turnAroundPlayer = undefined;
            this.removeAllButtonsActiveClass();
            this.setButtonsActiveClass();
        }
    }

    private removeAllButtonsActiveClass() {
        const children = this.carousel.nativeElement.children as HTMLCollection;
        for (let i = 0; i < children.length; ++i) {
            children.item(i).classList.remove("active");
        }
    }

    private removeButtonsActiveClass() {
        const children = this.carousel.nativeElement.children as HTMLCollection;
        children.item(this.buttons.length - this.selectedIndex - 1).classList.remove("active");
    }

    private setButtonsActiveClass() {
        const children = this.carousel.nativeElement.children as HTMLCollection;
        children.item(this.buttons.length - this.selectedIndex - 1).classList.add("active");
    }
}
