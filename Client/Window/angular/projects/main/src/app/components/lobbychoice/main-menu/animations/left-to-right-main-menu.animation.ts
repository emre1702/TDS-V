import { trigger, transition, style, animate, state } from "@angular/animations";

export const leftToRightMainMenuAnimation = trigger('leftToRight', [
    state("void", style({ transform: 'translateX(-100%)', opacity: 0 })),
    state("close", style({ transform: 'translateX(-100%)', opacity: 0 })),
    transition('* => open', [
        animate('500ms', style({ transform: 'translateX(0)', opacity: 0.9 }))
    ]),
    transition('* => close', [
        animate('500ms', style({ transform: 'translateX(-100%)', opacity: 0 })),
    ])
]);
