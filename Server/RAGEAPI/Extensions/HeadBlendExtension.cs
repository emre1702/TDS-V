namespace TDS_Server.RAGEAPI.Extensions
{
    internal static class HeadBlendExtension
    {
        #region Public Methods

        public static GTANetworkAPI.HeadBlend ToMod(this Data.Models.HeadBlend headBlend)
        {
            return new GTANetworkAPI.HeadBlend
            {
                ShapeFirst = headBlend.ShapeFirst,
                ShapeSecond = headBlend.ShapeSecond,
                ShapeThird = headBlend.ShapeThird,
                SkinFirst = headBlend.SkinFirst,
                SkinSecond = headBlend.SkinSecond,
                SkinThird = headBlend.SkinThird,
                ShapeMix = headBlend.ShapeMix,
                SkinMix = headBlend.SkinMix,
                ThirdMix = headBlend.ThirdMix
            };
        }

        public static Data.Models.HeadBlend ToTDS(this GTANetworkAPI.HeadBlend headBlend)
        {
            return new Data.Models.HeadBlend
            {
                ShapeFirst = headBlend.ShapeFirst,
                ShapeSecond = headBlend.ShapeSecond,
                ShapeThird = headBlend.ShapeThird,
                SkinFirst = headBlend.SkinFirst,
                SkinSecond = headBlend.SkinSecond,
                SkinThird = headBlend.SkinThird,
                ShapeMix = headBlend.ShapeMix,
                SkinMix = headBlend.SkinMix,
                ThirdMix = headBlend.ThirdMix
            };
        }

        #endregion Public Methods
    }
}
