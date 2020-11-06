﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Database.EntityConfigurations
{
    public static class Enums
    {
        public static ModelBuilder HasEnums(this ModelBuilder modelBuilder)
        {
            return modelBuilder
                .HasPostgresEnum<PlayerRelation>()
                .HasPostgresEnum<WeaponHash>()
                .HasPostgresEnum<WeaponType>()
                .HasPostgresEnum<LogType>()
                .HasPostgresEnum<LobbyType>()
                .HasPostgresEnum<Language>()
                .HasPostgresEnum<VehicleHash>()
                .HasPostgresEnum<FreeroamVehicleType>()
                .HasPostgresEnum<MapLimitType>()
                .HasPostgresEnum<RuleCategory>()
                .HasPostgresEnum<RuleTarget>()
                .HasPostgresEnum<UserpanelAdminQuestionAnswerType>()
                .HasPostgresEnum<SupportType>()
                .HasPostgresEnum<ChallengeType>()
                .HasPostgresEnum<ChallengeFrequency>()
                .HasPostgresEnum<ScoreboardPlayerSorting>()
                .HasPostgresEnum<TimeSpanUnitsOfTime>()
                .HasPostgresEnum<PedBodyPart>();
        }
    }
}