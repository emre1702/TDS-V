using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Account;

namespace TDS_Client.Manager.Draw
{
    internal static class MoneyDisplay
    {
        private static DxText moneyText;

        private static DxText GetMoneyText()
        {
            if (moneyText != null)
                return moneyText;
            return new DxText("0", Dx.ResX - 5, Dx.ResY - 5, 0.5f, Color.FromArgb(115, 186, 131), alignmentX: UIResText.Alignment.Right, alignmentY: Enum.EAlignmentY.Bottom,
                font: Font.Pricedown, dropShadow: true, outline: true, relative: false);
        }

        public static void Refresh()
        {
            if (moneyText == null)
                moneyText = GetMoneyText();
            moneyText.Text = AccountData.Money.ToString();
        }
    }
}