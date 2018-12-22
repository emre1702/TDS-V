using TDS_Client.Interface;

namespace TDS_Client.Instance.Language
{
    class OrderTextsEnglish : IOrderTexts
    {
        public virtual string ATTACK => "Attack! Go go go!";

        public virtual string BACK => "Stay back!";

        public virtual string SPREAD_OUT => "Spread out!";

        public virtual string TO_BOMB => "Go to the bomb!";
    }
}
