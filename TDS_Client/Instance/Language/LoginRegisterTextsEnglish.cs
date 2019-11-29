using MessagePack;
using TDS_Client.Interface;

namespace TDS_Client.Instance.Language
{
    [MessagePackObject]
    public class LoginRegisterTextsEnglish : ILoginRegisterTexts
    {
        [Key(0)]
        public virtual string tab_login => "Login";

        [Key(1)]
        public virtual string tab_register => "Register";

        [Key(2)]
        public virtual string username => "Username";

        [Key(3)]
        public virtual string password => "Password";

        [Key(4)]
        public virtual string login_title => "Welcome back";

        [Key(5)]
        public virtual string login_forgot_pw => "Password forgotten?";

        [Key(6)]
        public virtual string login_button => "Login";

        [Key(7)]
        public virtual string register_title => "Register at TDS";

        [Key(8)]
        public virtual string register_email => "E-Mail Address for password reset";

        [Key(9)]
        public virtual string register_confirm_pw => "Confirm password";

        [Key(10)]
        public virtual string register_button => "Send";

        [Key(11)]
        public virtual string forgotpw_title => "Reset your password";

        [Key(12)]
        public virtual string forgotpw_email => "E-Mail Address";

        [Key(13)]
        public virtual string forgotpw_reset => "Reset";

        [Key(14)]
        public virtual string password_has_to_be_same => "Both passwords have to be the same.";

        [Key(15)]
        public virtual string name_may_not_only_numbers => "The name must not consist only of numbers.";
    }
}