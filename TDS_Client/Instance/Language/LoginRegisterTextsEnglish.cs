using TDS_Client.Interface;

namespace TDS_Client.Instance.Language
{
    class LoginRegisterTextsEnglish : ILoginRegisterTexts
    {
        public virtual string TAB_LOGIN => "Login";

        public virtual string TAB_REGISTER => "Register";

        public virtual string USERNAME => "Username";

        public virtual string PASSWORD => "Password";

        public virtual string LOGIN_TITLE => "Welcome back";

        public virtual string LOGIN_FORGOT_PW => "Password forgotten?";

        public virtual string LOGIN_BUTTON => "Login";

        public virtual string REGISTER_TITLE => "Register at TDS";

        public virtual string REGISTER_EMAIL => "E-Mail Address for password reset";

        public virtual string REGISTER_CONFIRM_PW => "Confirm password";

        public virtual string REGISTER_BUTTON => "Send";

        public virtual string FORGOTPW_TITLE => "Reset your password";

        public virtual string FORGOTPW_EMAIL => "E-Mail Address";

        public virtual string FORGOTPW_RESET => "Reset";

        public virtual string PASSWORD_HAS_TO_BE_SAME => "Both passwords have to be the same";
    }
}
