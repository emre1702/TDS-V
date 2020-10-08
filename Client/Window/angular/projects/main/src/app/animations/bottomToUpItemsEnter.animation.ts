import { trigger, transition, style, animate, query, stagger } from "@angular/animations";

export const bottomToTopItemsEnterAnimation = trigger('bottomToTopItemsEnter', [
    transition('* => *', [
        query(':enter', [
            style({ transform: 'translateY(100%)', opacity: 0 }),
            stagger('150ms',
                animate('500ms', style({ transform: 'translateY(0)', opacity: 1 })),
            )
        ], { optional: true }),
        query(':leave', [
            style({ transform: 'translateY(0)', opacity: 1 }),
            stagger('150ms',
                animate('500ms', style({ transform: 'translateY(100%)', opacity: 0 }))
            )
        ], { optional: true })
    ])
]);

