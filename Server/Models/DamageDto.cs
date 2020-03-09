﻿using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server.Dto
{
    internal class DamageDto
    {
        public float Damage;
        public float HeadMultiplier;

        public DamageDto()
        {
        }

        public DamageDto(LobbyWeapons weapon)
        {
            Damage = weapon.Damage ?? 0;
            HeadMultiplier = weapon.HeadMultiplicator ?? 0;
        }
    }
}