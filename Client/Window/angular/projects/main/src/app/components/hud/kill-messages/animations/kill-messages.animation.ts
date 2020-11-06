import { trigger, transition, query, style, stagger, animate } from '@angular/animations';

export const killMessagesAnimation = trigger('killMessages', [
    transition('* => *', [
        query(':enter', [
            style({ transform: 'translateX(100%)', opacity: 0 }),
            stagger(150, [
                animate('800ms ease-out', style({ transform: 'translateX(0)', opacity: 1 })),
            ])
        ], { optional: true }),
        query(':leave', [
            style({ transform: 'translateX(0)', opacity: 1 }),
            stagger(150, [
                animate('800ms', style({ transform: 'translateX(100%)', opacity: 0 }))
            ])
        ], { optional: true })
    ])
]);
