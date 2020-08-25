import LoginRegisterTexts from "../../interfaces/output/login-register-texts.interface";
import LoginRegisterEnglish from "./login-register.english.language";

export default class LoginRegisterGerman extends LoginRegisterEnglish implements LoginRegisterTexts {
    forgotpw_email = "E-Mail Adresse";
    forgotpw_reset = "Zurücksetzen";
    forgotpw_title = "Passwort zurücksetzen";
    login_button = "Einloggen";
    login_forgot_pw = "Passwort vergessen?";
    login_title = "Willkommen zurück";
    name_may_not_only_numbers = "Der Name darf nicht nur aus Zahlen bestehen.";
    password = "Passwort";
    password_has_to_be_same = "Beide Passwörter müssen gleich sein.";
    register_button = "Abschicken";
    register_confirm_pw = "Passwort bestätigen";
    register_email = "E-Mail Adresse für Password-Reset";
    register_title = "Registriere dich auf TDS";
    tab_login = "Login";

    tab_register = "Register";

    username = "Benutzername";
}
