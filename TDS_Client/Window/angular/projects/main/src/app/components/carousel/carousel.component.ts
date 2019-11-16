import { Component, OnInit, Input, ViewChild, ChangeDetectorRef, AfterViewInit, ElementRef, HostListener } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { SettingsService } from '../../services/settings.service';
import { trigger, transition, query, style, stagger, animate, AnimationBuilder, AnimationPlayer } from '@angular/animations';
import { LobbyChoice } from '../lobbychoice/lobby-choice/interfaces/lobby-choice';

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
        this.changeCarousel();
        this.changeDetector.detectChanges();
        this.createTurnAroundAnimation();
    }

    private changeCarousel() {
        const cellCount = this.buttons.length;
        this.theta = 360 / cellCount;
        const cellSize = window.innerWidth * 0.8;
        this.radius = Math.round((cellSize / 2) / Math.tan(Math.PI / cellCount));
        for (let i = 0; i < cellCount; i++) {
            let btnStyle = this.buttonStyles[i + 1];
            if (!btnStyle) {
                btnStyle = { transform: "" };
                this.buttonStyles[i + 1] = btnStyle;
            }
            const cellAngle = this.theta * i;
            btnStyle.transform = 'rotateY(' + cellAngle + 'deg) translateZ(' + this.radius + 'px)';
            console.log(btnStyle);
        }

        this.rotateCarousel();
    }

    private rotateCarousel() {
        const btnStyle = this.buttonStyles[0];
        console.log(this.angle);
        btnStyle.transform = 'translateZ(' + -this.radius + 'px) rotateY(' + this.angle + 'deg)';
    }

    private createTurnAroundAnimation() {
        const anim = this.animationBuilder.build([
            style({ transform: 'translateZ(' + -this.radius + 'px) rotateY(' + this.angle + 'deg)' }),
            animate(1300 * this.buttons.length - 200 * this.buttons.length, style({ transform: 'translateZ(' + -this.radius + 'px) rotateY(360deg)' }))
        ]);

        this.turnAroundPlayer = anim.create(this.carousel.nativeElement);
        this.turnAroundPlayer.play();

        this.turnAroundPlayer.onDone(() => {
            if (this.turnAroundPlayer) {
                this.turnAroundPlayer.destroy();
                this.turnAroundPlayer = undefined;
            }
        });
    }

    getImageUrl(url: string) {
        return this.sanitizer.bypassSecurityTrustStyle("url(" + url + ") no-repeat");
    }

    @HostListener('window:keyup', ['$event'])
    keyEvent(event: KeyboardEvent) {
        console.log(event.key);
        if (event.key === "ArrowRight") {
            this.toRight();
        } else if (event.key === "ArrowLeft") {
            this.toLeft();
        } else if (event.key === "Enter") {
            this.buttons[this.selectedIndex].func();
        } else if (event.key === " ") {
            if (this.turnAroundPlayer) {
                this.turnAroundPlayer.destroy();
                this.turnAroundPlayer = undefined;
            }
        }
    }

    toRight() {
        if (this.turnAroundPlayer) {
            this.turnAroundPlayer.destroy();
            this.turnAroundPlayer = undefined;
        }
        this.angle -= this.theta;
        this.selectedIndex = this.selectedIndex + 1 >= this.buttons.length ? 0 : this.selectedIndex + 1;
        this.rotateCarousel();
        this.changeDetector.detectChanges();
    }

    toLeft() {
        if (this.turnAroundPlayer) {
            this.turnAroundPlayer.destroy();
            this.turnAroundPlayer = undefined;
        }
        this.selectedIndex = this.selectedIndex - 1 < 0 ? this.buttons.length - 1 : this.selectedIndex - 1;
        this.angle += this.theta;
        this.rotateCarousel();
        this.changeDetector.detectChanges();
    }
}
