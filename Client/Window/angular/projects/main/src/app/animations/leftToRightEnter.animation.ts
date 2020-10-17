import { trigger, transition, style, animate } from "@angular/animations";

export const leftToRightEnterAnimation = trigger('leftToRightEnter', [
    transition(':enter', [
        style({ transform: 'translateX(-100%)', opacity: 0 }),
        animate('500ms', style({ transform: 'translateX(0)', opacity: 0.9 }))
    ]),
    transition(':leave', [
        animate('500ms', style({ transform: 'translateX(-100%)', opacity: 0 })),
    ])
]);

