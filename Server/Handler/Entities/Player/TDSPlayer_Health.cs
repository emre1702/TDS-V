namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private int _armor;
        private int _health;

        #endregion Private Fields

        #region Public Properties

        public int Armor
        {
            get => _armor;
            set
            {
                _armor = value;

                if (ModPlayer is null)
                    return;
                ModPlayer.Armor = value;
            }
        }

        public int Health
        {
            get => _health;
            set
            {
                _health = value;

                if (ModPlayer is null)
                    return;
                ModPlayer.Health = value;
            }
        }

        #endregion Public Properties
    }
}
