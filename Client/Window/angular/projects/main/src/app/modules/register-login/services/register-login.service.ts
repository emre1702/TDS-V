import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegisterLoginInitData } from '../models/register-login-init-data';

export abstract class RegisterLoginService {
    abstract loadRegisterLoginInitData(): Observable<RegisterLoginInitData>;
}
