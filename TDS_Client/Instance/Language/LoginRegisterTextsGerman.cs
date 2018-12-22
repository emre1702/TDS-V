using TDS_Client.Interface;

namespace TDS_Client.Instance.Language
{
    class LoginRegisterTextsGerman : LoginRegisterTextsEnglish
    {
        public override string TAB_LOGIN => "Login";

        public override string TAB_REGISTER => "Register";

        public override string USERNAME => "Benutzername";

        public override string PASSWORD => "Passwort";

        public override string LOGIN_TITLE => "Willkommen zurueck";

        public override string LOGIN_FORGOT_PW => "Passwort vergessen?";

        public override string LOGIN_BUTTON => "Einloggen";

        public override string REGISTER_TITLE => "Registriere dich auf TDS";

        public override string REGISTER_EMAIL => "E-Mail Adresse für Password-Reset";

        public override string REGISTER_CONFIRM_PW => "Passwort bestaetigen";

        public override string REGISTER_BUTTON => "Abschicken";

        public override string FORGOTPW_TITLE => "Passwort zurücksetzen";

        public override string FORGOTPW_EMAIL => "E-Mail Adresse";

        public override string FORGOTPW_RESET => "Zuruecksetzen";

        public override string PASSWORD_HAS_TO_BE_SAME => "Beide Passwoerter muessen gleich sein.";
    }
}
