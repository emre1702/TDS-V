import { trigger, transition, style, animate, query, stagger, state } from "@angular/animations";

export const topToBottomMainMenuAnimation = trigger('topToBottom', [
    transition("* => *", [
        query(":enter", [
            style({ transform: 'translateY(-90vh)', opacity: 0 }),
            stagger("150ms", animate('800ms', style({ transform: 'translateY(0)', opacity: 0.9 })))
        ], { optional: true }),
        query(":leave", [
            style({ transform: 'translateY(0)', opacity: 0.9 }),
            stagger("-150ms", animate('800ms', style({ transform: 'translateY(-90vh)', opacity: 0 })))
        ], { optional: true }),
    ])
]);

