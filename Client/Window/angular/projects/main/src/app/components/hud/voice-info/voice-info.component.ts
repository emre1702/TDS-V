import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { VoiceInfo } from './models/voice-info';
import { RageConnectorService } from 'rage-connector';
import { DFromClientEvent } from '../../../enums/dfromclientevent.enum';
import { InitialDatas } from '../../../initial-datas';

@Component({
    selector: 'app-voice-info',
    templateUrl: './voice-info.component.html',
    styleUrls: ['./voice-info.component.scss'],
})
export class VoiceInfoComponent implements OnInit, OnDestroy {
    voiceInfos: VoiceInfo[] = InitialDatas.getVoiceInfos();

    constructor(private rageConnector: RageConnectorService, private changeDetector: ChangeDetectorRef) {}

    ngOnInit() {
        this.rageConnector.listen(DFromClientEvent.AddUserTalking, this.addUserTalking.bind(this));
        this.rageConnector.listen(DFromClientEvent.RemoveUserTalking, this.removeUserTalking.bind(this));
    }

    ngOnDestroy() {
        this.rageConnector.remove(DFromClientEvent.AddUserTalking, this.addUserTalking.bind(this));
        this.rageConnector.remove(DFromClientEvent.RemoveUserTalking, this.removeUserTalking.bind(this));
    }

    private addUserTalking(remoteId: number, name: string) {
        this.voiceInfos.push({ RemoteId: remoteId, Name: name });
        this.changeDetector.detectChanges();
    }

    private removeUserTalking(remoteId: number) {
        const index = this.voiceInfos.findIndex((info) => info[0] == remoteId);
        if (index >= 0) {
            this.voiceInfos.splice(index, 1);
        }
        this.changeDetector.detectChanges();
    }
}
