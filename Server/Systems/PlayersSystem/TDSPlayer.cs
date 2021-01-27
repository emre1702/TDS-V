using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Handler;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Enums;

namespace TDS.Server.PlayersSystem
{
    public class TDSPlayer : ITDSPlayer
    {
        public override IPlayerAdmin Admin { get; }
        public override IPlayerChallengesHandler Challenges { get; }
        public override IPlayerChat Chat { get; }
        public override IPlayerDatabaseHandler DatabaseHandler { get; }
        public override IPlayerDeathmatch Deathmatch { get; }
        public override IPlayerEvents Events { get; }
        public override IPlayerGangHandler GangHandler { get; }
        public override IPlayerHealthAndArmor HealthAndArmor { get; }
        public override IPlayerLanguageHandler LanguageHandler { get; }
        public override IPlayerLobbyHandler LobbyHandler { get; }
        public override IPlayerMapsVoting MapsVoting { get; }
        public override IPlayerMoneyHandler MoneyHandler { get; }
        public override IPlayerMuteHandler MuteHandler { get; }
        public override IPlayerPlayTime PlayTime { get; }
        public override IPlayerRelations Relations { get; }
        public override IPlayerSpectateHandler SpectateHandler { get; }
        public override IPlayerSync Sync { get; }
        public override IPlayerTeamHandler TeamHandler { get; }
        public override IPlayerTimezone Timezone { get; }
        public override IPlayerVoice Voice { get; }
        public override IPlayerWeaponStats WeaponStats { get; }

        private readonly IWorkaroundsHandler _workaroundsHandler;

        public TDSPlayer(
            NetHandle netHandle,

            IWorkaroundsHandler workaroundsHandler,

            IPlayerAdmin admin,
            IPlayerChallengesHandler challengesHandler,
            IPlayerChat chat,
            IPlayerDatabaseHandler databaseHandler,
            IPlayerDeathmatch deathmatch,
            IPlayerEvents events,
            IPlayerGangHandler gangHandler,
            IPlayerHealthAndArmor healthAndArmor,
            IPlayerLanguageHandler languageHandler,
            IPlayerLobbyHandler lobbyHandler,
            IPlayerMapsVoting mapsVoting,
            IPlayerMoneyHandler moneyHandler,
            IPlayerMuteHandler muteHandler,
            IPlayerPlayTime playTime,
            IPlayerRelations relations,
            IPlayerSpectateHandler spectateHandler,
            IPlayerSync sync,
            IPlayerTeamHandler teamHandler,
            IPlayerTimezone timezone,
            IPlayerVoice voice,
            IPlayerWeaponStats weaponStats) : base(netHandle)
        {
            try
            {
                _workaroundsHandler = workaroundsHandler;

                Events = events;
                events.Init(this);

                Admin = admin;
                admin.Init(this);

                Challenges = challengesHandler;
                challengesHandler.Init(this, events);

                Chat = chat;
                chat.Init(this);

                DatabaseHandler = databaseHandler;
                databaseHandler.Init(this);

                Deathmatch = deathmatch;
                deathmatch.Init(this, events);

                GangHandler = gangHandler;

                HealthAndArmor = healthAndArmor;
                healthAndArmor.Init(this);

                LanguageHandler = languageHandler;
                languageHandler.Init(this, events);

                LobbyHandler = lobbyHandler;
                lobbyHandler.Init(this);

                MapsVoting = mapsVoting;
                mapsVoting.Init(this);

                MoneyHandler = moneyHandler;
                moneyHandler.Init(this);

                MuteHandler = muteHandler;
                muteHandler.Init(this);

                PlayTime = playTime;
                playTime.Init(this);

                Relations = relations;
                relations.Init(this, events);

                SpectateHandler = spectateHandler;
                spectateHandler.Init(this);

                Sync = sync;
                sync.Init(this, events);

                TeamHandler = teamHandler;
                teamHandler.Init(this);

                Timezone = timezone;
                timezone.Init(this, events);

                Voice = voice;
                voice.Init(this);

                WeaponStats = weaponStats;
                weaponStats.Init(this, events);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public override void Spawn(Vector3 pos, float heading = 0)
        {
            DeathSpawnTimer?.Kill();
            DeathSpawnTimer = null;
            if (pos is null)
            {
                LoggingHandler.Instance?.LogError("Pos is null.", Environment.StackTrace, source: this);
                return;
            }

            NAPI.Task.RunSafe(() =>
            {
                NAPI.Player.SpawnPlayer(this, pos, heading);
                if (Lobby is { })
                    Dimension = Lobby.MapHandler.Dimension;
            });
        }

        public override void SetEntityInvincible(ITDSVehicle vehicle, bool invincible)
            => vehicle.SetInvincible(invincible, this);

        public override void AttachTo(ITDSPlayer player, PedBone bone, Vector3? positionOffset, Vector3? rotationOffset)
            => _workaroundsHandler.AttachEntityToEntity(this, player, bone, positionOffset ?? new Vector3(), rotationOffset ?? new Vector3(), player.Lobby);

        public override void Detach()
            => _workaroundsHandler.DetachEntity(this);

        public override void Freeze(bool toggle)
            => _workaroundsHandler.FreezePlayer(this, toggle);

        public override void SetCollisionsless(bool toggle)
            => _workaroundsHandler.SetEntityCollisionless(this, toggle, Lobby);

        public override void SetInvincible(bool toggle)
            => _workaroundsHandler.SetPlayerInvincible(this, toggle);

        public override void SetInvisible(bool toggle)
        {
            if (toggle)
            {
                Transparency = 0;
                SetCollisionsless(true);
            }
            else
            {
                Transparency = 255;
                SetCollisionsless(false);
            }
        }
    }
}