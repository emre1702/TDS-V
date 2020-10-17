import { trigger, transition, style, animate, query, stagger } from "@angular/animations";

export const topToBottomItemsEnterAnimation = trigger('topToBottomItemsEnter', [
    transition('* => *', [
        query(':enter', [
            style({ transform: 'translateY(-500%)', opacity: 0}),
            stagger('250ms',
                animate('1000ms', style({ transform: 'translateY(0)', opacity: 1 })),
            )
        ], { optional: true }),
        query(':leave', [
            style({ transform: 'translateY(0)', opacity: 1 }),
            stagger('250ms',
                animate('1000ms', style({ transform: 'translateY(-500%)', opacity: 0 }))
            )
        ], { optional: true })
    ])
]);

