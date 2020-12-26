import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterLoginComponent } from './register-login.component';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { PasswordForgottenComponent } from './components/password-forgotten/password-forgotten.component';
import { TDSWindowModule } from '../tds-window/tds-window.module';

@NgModule({
    declarations: [RegisterLoginComponent, LoginComponent, RegisterComponent, PasswordForgottenComponent],
    exports: [RegisterLoginComponent],
    imports: [CommonModule, FormsModule, MaterialModule, SharedModule, TDSWindowModule],
})
export class RegisterLoginModule {}
