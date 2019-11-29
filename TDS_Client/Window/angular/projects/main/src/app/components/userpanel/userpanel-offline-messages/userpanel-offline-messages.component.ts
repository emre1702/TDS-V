import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { UserpanelService } from '../services/userpanel.service';
import { RageConnectorService } from 'rage-connector';
import { SettingsService } from '../../../services/settings.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';

@Component({
    selector: 'app-userpanel-offline-messages',
    templateUrl: './userpanel-offline-messages.component.html',
    styleUrls: ['./userpanel-offline-messages.component.scss']
})
export class UserpanelOfflineMessagesComponent implements OnInit, OnDestroy {
    inOfflineMessage: [
        /** ID */
        number,
        /** PlayerName */
        string,
        /** CreateTime */
        string,
        /** Text */
        string,
        /** Seen */
        boolean
    ] = undefined;
    clickedOfflineMessage: number = undefined;
    creatingOfflineMessage = false;

    displayedColumns = ["PlayerName", "Text", "CreateTime", "Delete"];
    offlineMessageFormGroup: FormGroup;

    constructor(
        public userpanelService: UserpanelService,
        private rageConnector: RageConnectorService,
        public settings: SettingsService,
        private changeDetector: ChangeDetectorRef
    ) {
        this.offlineMessageFormGroup = new FormGroup({
            playerName: new FormControl(''),
            message: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(255)])
        });
    }

    ngOnInit() {
        this.userpanelService.offlineMessagesLoaded.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.userpanelService.offlineMessagesLoaded.off(null, this.detectChanges.bind(this));
    }

    detectChanges() {
        this.changeDetector.detectChanges();
    }

    selectRow(row: { ID: number }) {
        this.clickedOfflineMessage = row.ID;
        this.changeDetector.detectChanges();
    }

    createNew() {
        this.offlineMessageFormGroup.get("playerName").setValue('');
        this.offlineMessageFormGroup.get("playerName").setValidators([Validators.required, Validators.minLength(3), Validators.maxLength(50)]);
        this.offlineMessageFormGroup.get("playerName").updateValueAndValidity({ onlySelf: true });
        this.creatingOfflineMessage = true;
        this.changeDetector.detectChanges();
    }

    open() {
        this.offlineMessageFormGroup.get("playerName").setValue('');
        this.offlineMessageFormGroup.get("playerName").clearValidators();
        this.offlineMessageFormGroup.get("playerName").updateValueAndValidity({ onlySelf: true });
        this.inOfflineMessage = this.userpanelService.offlineMessages.find(o => o[0] == this.clickedOfflineMessage);
        this.changeDetector.detectChanges();
    }

    backNav() {
        this.inOfflineMessage = undefined;
        this.creatingOfflineMessage = false;
        this.changeDetector.detectChanges();
    }

    answer() {
        if (this.offlineMessageFormGroup.invalid) {
            return;
        }
        this.rageConnector.callServer(DToServerEvent.AnswerToOfflineMessage,
            this.inOfflineMessage[0],
            this.offlineMessageFormGroup.get("message").value);
        this.offlineMessageFormGroup.get("message").setValue("");
    }

    sendMessage() {
        if (this.offlineMessageFormGroup.invalid) {
            return;
        }
        this.rageConnector.callCallbackServer(DToServerEvent.SendOfflineMessage,
            [this.offlineMessageFormGroup.get("playerName").value,
            this.offlineMessageFormGroup.get("message").value], (bool: boolean) => {
                if (bool) {
                    this.offlineMessageFormGroup.get("message").setValue("");
                }
            });
    }

    delete(id: number) {
        this.userpanelService.offlineMessages = [...this.userpanelService.offlineMessages.filter(o => o[0] !== id)];

        this.rageConnector.callServer(DToServerEvent.DeleteOfflineMessage, id);
    }
}
