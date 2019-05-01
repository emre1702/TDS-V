using TDS_Client.Interface;

namespace TDS_Client.Instance.Language
{
    internal class LoginRegisterTextsEnglish : ILoginRegisterTexts
    {
        public virtual string tab_login => "Login";

        public virtual string tab_register => "Register";

        public virtual string username => "Username";

        public virtual string password => "Password";

        public virtual string login_title => "Welcome back";

        public virtual string login_forgot_pw => "Password forgotten?";

        public virtual string login_button => "Login";

        public virtual string register_title => "Register at TDS";

        public virtual string register_email => "E-Mail Address for password reset";

        public virtual string register_confirm_pw => "Confirm password";

        public virtual string register_button => "Send";

        public virtual string forgotpw_title => "Reset your password";

        public virtual string forgotpw_email => "E-Mail Address";

        public virtual string forgotpw_reset => "Reset";

        public virtual string password_has_to_be_same => "Both passwords have to be the same";
    }
}