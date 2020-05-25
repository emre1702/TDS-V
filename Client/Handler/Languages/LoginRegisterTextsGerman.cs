namespace TDS_Client.Handler.Entities.Languages
{
    internal class LoginRegisterTextsGerman : LoginRegisterTextsEnglish
    {
        #region Public Properties

        public override string forgotpw_email => "E-Mail Adresse";
        public override string forgotpw_reset => "Zurücksetzen";
        public override string forgotpw_title => "Passwort zurücksetzen";
        public override string login_button => "Einloggen";
        public override string login_forgot_pw => "Passwort vergessen?";
        public override string login_title => "Willkommen zurück";
        public override string name_may_not_only_numbers => "Der Name darf nicht nur aus Zahlen bestehen.";
        public override string password => "Passwort";
        public override string password_has_to_be_same => "Beide Passwörter müssen gleich sein.";
        public override string register_button => "Abschicken";
        public override string register_confirm_pw => "Passwort bestätigen";
        public override string register_email => "E-Mail Adresse für Password-Reset";
        public override string register_title => "Registriere dich auf TDS";
        public override string tab_login => "Login";

        public override string tab_register => "Register";

        public override string username => "Benutzername";

        #endregion Public Properties
    }
}
