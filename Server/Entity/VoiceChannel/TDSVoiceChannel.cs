using System;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.VoiceChannel
{
    //Add this to teams
    public class TDSVoiceChannel : AltV.Net.Elements.Entities.VoiceChannel, ITDSVoiceChannel
    {

        public TDSVoiceChannel(IntPtr nativePointer) : base(nativePointer) { }
    }
}
