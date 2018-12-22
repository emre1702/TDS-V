namespace TDS_Client.Interface
{
    interface ILoginRegisterTexts
    {
        string TAB_LOGIN { get; }
        string TAB_REGISTER { get; }
        string USERNAME { get; }
        string PASSWORD { get; }
        string LOGIN_TITLE { get; }
        string LOGIN_FORGOT_PW { get; }
        string LOGIN_BUTTON { get; }
        string REGISTER_TITLE { get; }
        string REGISTER_EMAIL { get; }
        string REGISTER_CONFIRM_PW { get; }
        string REGISTER_BUTTON { get; }
        string FORGOTPW_TITLE { get; }
        string FORGOTPW_EMAIL { get; }
        string FORGOTPW_RESET { get; }
        string PASSWORD_HAS_TO_BE_SAME { get; }
    }
}
