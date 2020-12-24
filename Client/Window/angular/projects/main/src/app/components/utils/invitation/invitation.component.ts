import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { Invitation } from './models/invitation';
import { RageConnectorService } from 'rage-connector';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { FromServerEvent } from '../../../enums/from-server-event.enum';
import { trigger, transition, query, style, stagger, animate } from '@angular/animations';

@Component({
    selector: 'app-invitation',
    templateUrl: './invitation.component.html',
    styleUrls: ['./invitation.component.scss'],
    animations: [
        trigger('invitationAnimation', [
            transition('* => *', [
                query(
                    ':enter',
                    [
                        style({ transform: 'translateX(100%)', opacity: 0 }),
                        stagger(400, [animate('800ms', style({ transform: 'translateX(0)', opacity: 0.9 }))]),
                    ],
                    { optional: true }
                ),
                query(
                    ':leave',
                    [
                        style({ transform: 'translateX(0)', opacity: 0.9 }),
                        stagger(400, [animate('800ms', style({ transform: 'translateX(100%)', opacity: 0 }))]),
                    ],
                    { optional: true }
                ),
            ]),
        ]),
    ],
})
export class InvitationComponent implements OnInit, OnDestroy {
    invitations: Invitation[] = [];

    constructor(public settings: SettingsService, private rageConnector: RageConnectorService, private changeDetector: ChangeDetectorRef) {}

    ngOnInit() {
        this.rageConnector.listen(FromServerEvent.AddInvitation, this.addInvitation.bind(this));
        this.rageConnector.listen(FromServerEvent.RemoveInvitation, this.removeInvitation.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(FromServerEvent.AddInvitation, this.addInvitation.bind(this));
        this.rageConnector.remove(FromServerEvent.RemoveInvitation, this.removeInvitation.bind(this));
    }

    accept(id: number) {
        this.rageConnector.callServer(ToServerEvent.AcceptInvitation, id);
        this.removeInvitation(id);
    }

    reject(id: number) {
        this.rageConnector.callServer(ToServerEvent.RejectInvitation, id);
        this.removeInvitation(id);
    }

    private addInvitation(invitationJson: string) {
        const invitation = JSON.parse(invitationJson);
        this.invitations.push(invitation);
        this.changeDetector.detectChanges();
    }

    private removeInvitation(invitationId: number) {
        const index = this.invitations.findIndex((i) => i[0] == invitationId);
        if (index >= 0) {
            this.invitations.splice(index, 1);
            this.changeDetector.detectChanges();
        }
    }
}
