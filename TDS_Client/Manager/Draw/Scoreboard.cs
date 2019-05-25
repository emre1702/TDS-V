using RAGE.Game;
using RAGE.NUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Instance.Draw.Dx.Grid;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using static RAGE.Events;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Draw
{
    internal class Scoreboard : Script
    {
        private static DxGrid grid;
        private static bool isActivated;
        private static ulong lastLoadedTick;

        private readonly static DxGridColumn[] columns = new DxGridColumn[6];

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
            grid = new DxGrid(0.5f, 0.5f, 0.45f, 0.365f, Color.FromArgb(187, 10, 10, 10), 0.3f, maxRows: 15);
            CreateColumns();
            CreateTitle();
            CreateBody();
            CreateFooter();
        }

        public static void AddMainmenuData(List<SyncedScoreboardMainmenuLobbyDataDto> list)
        {
            if (grid.Header != null)
                grid.Header.Activated = false;

            Color backLobbyOfficialColor = Color.FromArgb(187, 190, 190, 190);
            Color backLobbyColor = Color.FromArgb(187, 140, 140, 140);

            list.Sort((a, b) => a.Id < b.Id && a.Id != 0 ? 1 : 0);
            foreach (var entry in list)
            {
                DxGridRow lobbynamerow = new DxGridRow(grid, null, entry.IsOfficial ? backLobbyOfficialColor : backLobbyOfficialColor, Color.Black,
                    $"{entry.LobbyName} ({entry.PlayersCount})", textAlignment: UIResText.Alignment.Left, scale: 0.3f);
                if (entry.PlayersStr != null)
                {
                    DxGridRow lobbyplayersrow = new DxGridRow(grid, null, Color.DarkGray, Color.White, entry.PlayersStr, scale: 0.25f);
                }
            }
        }

        public static void AddLobbyData(List<SyncedScoreboardLobbyDataDto> playerlist, List<SyncedScoreboardMainmenuLobbyDataDto> lobbylist)
        {
            playerlist.Sort((a, b) => a.TeamIndex.CompareTo(b.TeamIndex));
            foreach (var playerdata in playerlist)
            {
                var team = Team.CurrentLobbyTeams[playerdata.TeamIndex];
                DxGridRow row = new DxGridRow(grid, null, team.Color, Color.White, textAlignment: UIResText.Alignment.Centered, scale: 0.3f);
                new DxGridCell(playerdata.Name, row, columns[0]);

                new DxGridCell(TimeSpan.FromMinutes(playerdata.PlaytimeMinutes).ToString(@"%h\:mm"), row, columns[1]);
                new DxGridCell(playerdata.Kills.ToString(), row, columns[2]);
                new DxGridCell(playerdata.Assists.ToString(), row, columns[3]);
                new DxGridCell(playerdata.Deaths.ToString(), row, columns[4]);
                new DxGridCell(team.Name, row, columns[5]);
            }

            AddMainmenuData(lobbylist);
            if (grid.Header != null)
                grid.Header.Activated = true;
        }

        public static void LoadLanguage()
        {
            if (grid.Header == null)
                return;
            grid.Header.Cells[0].SetText(Settings.Language.SCOREBOARD_NAME);
            grid.Header.Cells[1].SetText(Settings.Language.SCOREBOARD_PLAYTIME);
            grid.Header.Cells[2].SetText(Settings.Language.SCOREBOARD_KILLS);
            grid.Header.Cells[3].SetText(Settings.Language.SCOREBOARD_ASSISTS);
            grid.Header.Cells[4].SetText(Settings.Language.SCOREBOARD_DEATHS);
            grid.Header.Cells[5].SetText(Settings.Language.SCOREBOARD_TEAM);
        }

        public static void PressedScoreboardKey(Control _)
        {
            if (IsActivated)
                return;
            grid.ScrollIndex = 0;
            ulong tick = TimerManager.ElapsedTicks;
            if (tick - lastLoadedTick >= (ulong)Constants.ScoreboardLoadCooldown)
            {
                lastLoadedTick = tick;
                grid.ClearRows();
                EventsSender.Send(DToServerEvent.RequestPlayersForScoreboard);
            }

            IsActivated = true;
        }

        public static void ReleasedScoreboardKey(Control _)
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
            columns[0] = new DxGridColumn(0.3f, grid);
            columns[1] = new DxGridColumn(0.15f, grid);
            columns[2] = new DxGridColumn(0.1f, grid);
            columns[3] = new DxGridColumn(0.1f, grid);
            columns[4] = new DxGridColumn(0.1f, grid);
            columns[5] = new DxGridColumn(0.25f, grid);
        }

        private static void CreateTitle()
        {
            DxGridRow header = new DxGridRow(grid, 0.035f, Color.FromArgb(187, 20, 20, 20), Color.White, textAlignment: UIResText.Alignment.Centered, isHeader: true);
            new DxGridCell(Settings.Language.SCOREBOARD_NAME, header, columns[0]);
            new DxGridCell(Settings.Language.SCOREBOARD_PLAYTIME, header, columns[1]);
            new DxGridCell(Settings.Language.SCOREBOARD_KILLS, header, columns[2]);
            new DxGridCell(Settings.Language.SCOREBOARD_ASSISTS, header, columns[3]);
            new DxGridCell(Settings.Language.SCOREBOARD_DEATHS, header, columns[4]);
            new DxGridCell(Settings.Language.SCOREBOARD_TEAM, header, columns[5]);
        }

        private static void CreateBody()
        {
        }

        private static void CreateFooter()
        {
        }
    }
}