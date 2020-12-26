import { FormGroup } from '@angular/forms';
import { RageConnectorService } from 'rage-connector';
import { NotificationService } from '../../shared/services/notification.service';

export class RegisterLoginBase {
    isLoading: boolean;
    formGroup: FormGroup;

    constructor(private rageConnector: RageConnectorService, private notificationService: NotificationService) {}

    send(eventName: string, args: any[]) {
        if (!this.formGroup.valid) {
            return;
        }
        this.isLoading = true;
        this.rageConnector.callCallback(eventName, args, (msg: string) => {
            this.isLoading = false;
            if (msg?.length) {
                this.notificationService.showError(msg);
            }
        });
    }
}
