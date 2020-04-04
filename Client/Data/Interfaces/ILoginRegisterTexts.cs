﻿namespace TDS_Client.Data.Interfaces
{
    public interface ILoginRegisterTexts
    {
        string tab_login { get; }
        string tab_register { get; }
        string username { get; }
        string password { get; }
        string login_title { get; }
        string login_forgot_pw { get; }
        string login_button { get; }
        string register_title { get; }
        string register_email { get; }
        string register_confirm_pw { get; }
        string register_button { get; }
        string forgotpw_title { get; }
        string forgotpw_email { get; }
        string forgotpw_reset { get; }
        string password_has_to_be_same { get; }
        string name_may_not_only_numbers { get; }
    }
}
