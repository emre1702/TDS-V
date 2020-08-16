namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private ushort _armor;
        private ushort _health;

        #endregion Private Fields

        #region Public Properties

        public new ushort Armor
        {
            get => _armor;
            set
            {
                _armor = value;
                base.Armor = value;
            }
        }

        public new ushort Health
        {
            get => _health;
            set
            {
                _health = value;
                base.Health = value;
            }
        }

        #endregion Public Properties
    }
}
