using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Add_LobbyArmsRaceWeapons_More : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LobbyArmsRaceWeapons",
                columns: new[] { "LobbyId", "AtKill", "WeaponHash" },
                values: new object[,]
                {
                    { -1, (short)0, WeaponHash.Microsmg },
                    { -1, (short)3, WeaponHash.Assaultrifle },
                    { -1, (short)16, null },
                    { -1, (short)15, WeaponHash.Revolver },
                    { -1, (short)14, WeaponHash.Heavypistol },
                    { -1, (short)1, WeaponHash.Assaultsmg },
                    { -1, (short)13, WeaponHash.Pistol50 },
                    { -1, (short)12, WeaponHash.Combatpistol },
                    { -1, (short)10, WeaponHash.Marksmanrifle },
                    { -1, (short)2, WeaponHash.Machinepistol },
                    { -1, (short)11, WeaponHash.Combatmg },
                    { -1, (short)8, WeaponHash.Assaultshotgun },
                    { -1, (short)7, WeaponHash.Pumpshotgun },
                    { -1, (short)6, WeaponHash.Specialcarbine },
                    { -1, (short)5, WeaponHash.Advancedrifle },
                    { -1, (short)4, WeaponHash.Carbinerifle },
                    { -1, (short)9, WeaponHash.Heavysniper }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)0 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)1 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)2 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)3 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)4 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)5 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)6 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)7 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)8 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)9 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)10 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)11 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)12 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)13 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)14 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)15 });

            migrationBuilder.DeleteData(
                table: "LobbyArmsRaceWeapons",
                keyColumns: new[] { "LobbyId", "AtKill" },
                keyValues: new object[] { -1, (short)16 }); 
        }
    }
}
