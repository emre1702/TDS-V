using RAGE;
using System.Drawing;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSPlayer : RAGE.Elements.Player
    {
        public Vector3 Rotation
        {
            get => GetRotation(2);
            set => SetRotation(value.X, value.Y, value.Z, 2, true);
        }

        public ITDSPlayer(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        public void SetHeadBlendPaletteColor(Color color, int type)
        {
            RAGE.Game.Invoker.Invoke((ulong)NativeHash.SET_HEAD_BLEND_PALETTE_COLOR, Handle, (int)color.R, (int)color.G, (int)color.B, type);
        }

        public void SetHeadBlendPaletteColor(ColorDto color, int type)
        {
            RAGE.Game.Invoker.Invoke((ulong)NativeHash.SET_HEAD_BLEND_PALETTE_COLOR, Handle, (int)color.R, (int)color.G, (int)color.B, type);
        }
    }
}
