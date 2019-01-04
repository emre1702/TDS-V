using RAGE;
using RAGE.Game;
using RAGE.NUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Instance.Draw.Dx.Grid;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using static RAGE.Events;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Draw
{
    class Scoreboard : Script
    {
        private const float maxPlayers = 25,
                            completeWidth = 0.4f,
                            bodyHeight = 0.37f,
                            titleHeight = 0.03f;

        private static DxGrid grid;
        private static bool isActivated;
        private static ulong lastLoadedTick;

        private static DxGridColumn[] columns = new DxGridColumn[6];

        public static bool IsActivated
        {
            get => isActivated;
            set
            {
                isActivated = value;
                grid.Activated = value;
                if (value)
                    Tick += OnTick;
                else
                    Tick -= OnTick;
            }
        }

        public Scoreboard()
        {
            grid = new DxGrid(0.5f, 0.5f, 0.4f, 0.37f, Color.FromArgb(187, 10, 10, 10));
            CreateColumns();
            CreateTitle();
            CreateBody();
            CreateFooter();
        }

        public static void AddMainmenuData(List<SyncedScoreboardMainmenuLobbyDataDto> list)
        {
            grid.Header.Activated = false;

            Color backLobbyOfficialColor = Color.FromArgb(187, 10, 10, 150);
            Color backLobbyColor = Color.FromArgb(187, 10, 10, 190);

            list.Sort((a, b) => a.Id < b.Id && a.Id != 0 ? 1 : 0);
            foreach (var entry in list)
            {
                DxGridRow lobbynamerow = new DxGridRow(null, entry.IsOfficial ? backLobbyOfficialColor : backLobbyOfficialColor, Color.White, $"{entry.LobbyName} ({entry.PlayersCount})");
                grid.AddRow(lobbynamerow);
                DxGridRow lobbyplayersrow = new DxGridRow(null, Color.DarkGray, Color.White, entry.PlayersStr);
                grid.AddRow(lobbyplayersrow);
            }
        }

        public static void AddLobbyData(List<SyncedScoreboardLobbyDataDto> playerlist, List<SyncedScoreboardMainmenuLobbyDataDto> lobbylist)
        {
            playerlist.Sort((a, b) => a.TeamIndex.CompareTo(b.TeamIndex));
            foreach (var playerdata in playerlist)
            {
                var team = Team.CurrentLobbyTeams[playerdata.TeamIndex];
                DxGridRow row = new DxGridRow(null, team.Color, Color.Black, alignment: UIResText.Alignment.Centered);

                DxGridCell namecell = new DxGridCell(playerdata.Name, row, columns[0]);
                row.AddCell(namecell);
                DxGridCell playtimecell = new DxGridCell(TimeSpan.FromMinutes(playerdata.PlaytimeMinutes).ToString(@"%h\:mm"), row, columns[1]);
                row.AddCell(playtimecell);
                DxGridCell killscell = new DxGridCell(playerdata.Kills.ToString(), row, columns[2]);
                row.AddCell(killscell);
                DxGridCell assistscell = new DxGridCell(playerdata.Assists.ToString(), row, columns[3]);
                row.AddCell(assistscell);
                DxGridCell deathscell = new DxGridCell(playerdata.Deaths.ToString(), row, columns[4]);
                row.AddCell(deathscell);
                DxGridCell teamcell = new DxGridCell(team.Name, row, columns[5]);
                row.AddCell(teamcell);
            }

            AddMainmenuData(lobbylist);
            grid.Header.Activated = true;
        }

        public static void LoadLanguage()
        {
            grid.Header.Cells[0].SetText(Settings.Language.SCOREBOARD_NAME);
            grid.Header.Cells[1].SetText(Settings.Language.SCOREBOARD_PLAYTIME);
            grid.Header.Cells[2].SetText(Settings.Language.SCOREBOARD_KILLS);
            grid.Header.Cells[3].SetText(Settings.Language.SCOREBOARD_ASSISTS);
            grid.Header.Cells[4].SetText(Settings.Language.SCOREBOARD_DEATHS);
            grid.Header.Cells[5].SetText(Settings.Language.SCOREBOARD_TEAM);
        }

        public static void PressedScoreboardKey(Control key)
        {
            if (IsActivated)
                return;
            grid.ScrollIndex = 0;
            ulong tick = TimerManager.ElapsedTicks;
            if (tick - lastLoadedTick >= (ulong)Constants.ScoreboardLoadCooldown)
            {
                lastLoadedTick = tick;
                grid.ClearRows();
                CallRemote(DToServerEvent.RequestPlayersForScoreboard);
            }

            IsActivated = true;
        }

        public static void ReleasedScoreboardKey(Control key)
        {
            if (!IsActivated)
                return;
            IsActivated = false;
        }

        private static void OnTick(List<TickNametagData> _)
        {
            Pad.DisableControlAction(0, (int)Control.SelectNextWeapon, true);
            Pad.DisableControlAction(0, (int)Control.SelectPrevWeapon, true);
        }

        private static void CreateColumns()
        {
            columns[0] = new DxGridColumn(0.3f);
            columns[1] = new DxGridColumn(0.15f);
            columns[2] = new DxGridColumn(0.1f);
            columns[3] = new DxGridColumn(0.1f);
            columns[4] = new DxGridColumn(0.1f);
            columns[5] = new DxGridColumn(0.25f);
        }

        private static void CreateTitle()
        {
            DxGridRow header = new DxGridRow(null, Color.FromArgb(187, 20, 20, 20), Color.White, alignment: UIResText.Alignment.Centered);
            grid.SetHeader(header);
            header.AddCell(new DxGridCell(Settings.Language.SCOREBOARD_NAME, header, columns[0]));
            header.AddCell(new DxGridCell(Settings.Language.SCOREBOARD_PLAYTIME, header, columns[1]));
            header.AddCell(new DxGridCell(Settings.Language.SCOREBOARD_KILLS, header, columns[2]));
            header.AddCell(new DxGridCell(Settings.Language.SCOREBOARD_ASSISTS, header, columns[3]));
            header.AddCell(new DxGridCell(Settings.Language.SCOREBOARD_DEATHS, header, columns[4]));
            header.AddCell(new DxGridCell(Settings.Language.SCOREBOARD_TEAM, header, columns[5]));
        }

        private static void CreateBody() {}

        private static void CreateFooter() {}
    }
}
