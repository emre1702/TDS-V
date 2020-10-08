import { trigger, transition, style, animate } from "@angular/animations";

export const bottomToTopEnterAnimation = trigger('bottomToTopEnter', [
    transition(':enter', [
        style({ transform: 'translateY(100%)', opacity: 0 }),
        animate('500ms', style({ transform: 'translateY(0)', opacity: 0.9 }))
    ]),
    transition(':leave', [
        animate('500ms', style({ transform: 'translateY(100%)', opacity: 0 })),
    ])
]);

