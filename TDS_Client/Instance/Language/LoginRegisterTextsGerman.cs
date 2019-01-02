using TDS_Client.Interface;

namespace TDS_Client.Instance.Language
{
    class LoginRegisterTextsGerman : LoginRegisterTextsEnglish
    {
        public override string tab_login => "Login";

        public override string tab_register => "Register";

        public override string username => "Benutzername";

        public override string password => "Passwort";

        public override string login_title => "Willkommen zurueck";

        public override string login_forgot_pw => "Passwort vergessen?";

        public override string login_button => "Einloggen";

        public override string register_title => "Registriere dich auf TDS";

        public override string register_email => "E-Mail Adresse für Password-Reset";

        public override string register_confirm_pw => "Passwort bestaetigen";

        public override string register_button => "Abschicken";

        public override string forgotpw_title => "Passwort zurücksetzen";

        public override string forgotpw_email => "E-Mail Adresse";

        public override string forgotpw_reset => "Zuruecksetzen";

        public override string password_has_to_be_same => "Beide Passwoerter muessen gleich sein.";
    }
}
