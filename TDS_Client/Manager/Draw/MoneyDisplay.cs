using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Account;

namespace TDS_Client.Manager.Draw
{
    internal static class MoneyDisplay
    {
        private static DxText _moneyText;

        private static DxText GetMoneyText()
        {
            if (_moneyText != null)
                return _moneyText;
            return new DxText("0", 1920 - 5, 1080 - 5, 0.5f, Color.FromArgb(115, 186, 131), alignmentX: UIResText.Alignment.Right, alignmentY: Enum.EAlignmentY.Bottom,
                font: Font.Pricedown, dropShadow: true, outline: true, relative: false);
        }

        public static void Refresh()
        {
            if (_moneyText == null)
                _moneyText = GetMoneyText();
            _moneyText.Text = AccountData.Money.ToString();
        }
    }
}