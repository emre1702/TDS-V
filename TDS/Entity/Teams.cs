using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Teams
    {
        public uint Id { get; set; }
        public uint Index { get; set; }
        public string Name { get; set; }
        public uint? Lobby { get; set; }
        public byte ColorR { get; set; }
        public byte ColorG { get; set; }
        public byte ColorB { get; set; }
        public byte BlipColor { get; set; }
        public uint SkinHash { get; set; }
        public bool IsSpectatorTeam { get; set; }

        public Lobbies LobbyNavigation { get; set; }
    }
}
