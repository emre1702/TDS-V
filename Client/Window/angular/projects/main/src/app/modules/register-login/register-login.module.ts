import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterLoginComponent } from './register-login.component';
import { TDSWindowModule } from '../tdswindow/tds-window.module';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { PasswordForgottenComponent } from './components/password-forgotten/password-forgotten.component';

@NgModule({
    declarations: [RegisterLoginComponent, LoginComponent, RegisterComponent, PasswordForgottenComponent],
    exports: [RegisterLoginComponent],
    imports: [CommonModule, FormsModule, TDSWindowModule, MaterialModule, SharedModule],
})
export class RegisterLoginModule {}
