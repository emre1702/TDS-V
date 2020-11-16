namespace TDS.Client.Data.Interfaces
{
    public interface ILoginRegisterTexts
    {
        #region Public Properties

        string forgotpw_email { get; }
        string forgotpw_reset { get; }
        string forgotpw_title { get; }
        string login_button { get; }
        string login_forgot_pw { get; }
        string login_title { get; }
        string name_may_not_only_numbers { get; }
        string password { get; }
        string password_has_to_be_same { get; }
        string register_button { get; }
        string register_confirm_pw { get; }
        string register_email { get; }
        string register_title { get; }
        string tab_login { get; }
        string tab_register { get; }
        string username { get; }

        #endregion Public Properties
    }
}
