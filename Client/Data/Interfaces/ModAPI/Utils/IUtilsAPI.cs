namespace TDS_Client.Data.Interfaces.ModAPI.Utils
{
    public interface IUtilsAPI
    {
        #region Public Methods

        void Settimera(int value);

        int Timera();

        void Wait(int ms);

        #endregion Public Methods
    }
}
