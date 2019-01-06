using RAGE.Game;
using RAGE.NUI;
using System.Drawing;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Account;

namespace TDS_Client.Manager.Draw
{
    static class MoneyDisplay
    {
        private static DxText moneyText;

        public static void Start()
        {
            moneyText = new DxText("0", Dx.ResX - 5, Dx.ResY - 5, 0.5f, Color.FromArgb(115, 186, 131), alignmentX: UIResText.Alignment.Right, alignmentY: Enum.EAlignmentY.Bottom, 
                font: Font.Pricedown, dropShadow: true, outline: true, relative: false); 
        }

        public static void Refresh()
        {
            moneyText.Text = AccountData.Money.ToString();
        }
    }
}
