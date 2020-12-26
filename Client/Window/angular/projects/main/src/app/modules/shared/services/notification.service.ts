import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { NotificationData } from '../interfaces/notification-data';

@Injectable()
export class NotificationService {
    notificationSubscription = new Subject<NotificationData>();

    showSuccess(msg: string, duration?: number) {
        this.notificationSubscription.next({
            message: msg,
            duration: duration,
        });
    }

    showError(msg: string, duration?: number) {
        this.notificationSubscription.next({
            message: msg,
            duration: duration,
        });
    }

    showInfo(msg: string, duration: number | undefined = 3000) {
        this.notificationSubscription.next({
            message: msg,
            duration: duration,
        });
    }
}
