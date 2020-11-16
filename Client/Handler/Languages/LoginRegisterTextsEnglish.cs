using TDS.Client.Data.Interfaces;

namespace TDS.Client.Handler.Entities.Languages
{
    public class LoginRegisterTextsEnglish : ILoginRegisterTexts
    {
        #region Public Properties

        public virtual string forgotpw_email => "E-Mail Address";
        public virtual string forgotpw_reset => "Reset";
        public virtual string forgotpw_title => "Reset your password";
        public virtual string login_button => "Login";
        public virtual string login_forgot_pw => "Password forgotten?";
        public virtual string login_title => "Welcome back";
        public virtual string name_may_not_only_numbers => "The name must not consist only of numbers.";
        public virtual string password => "Password";
        public virtual string password_has_to_be_same => "Both passwords have to be the same.";
        public virtual string register_button => "Send";
        public virtual string register_confirm_pw => "Confirm password";
        public virtual string register_email => "E-Mail Address for password reset";
        public virtual string register_title => "Register at TDS";
        public virtual string tab_login => "Login";

        public virtual string tab_register => "Register";

        public virtual string username => "Username";

        #endregion Public Properties
    }
}
