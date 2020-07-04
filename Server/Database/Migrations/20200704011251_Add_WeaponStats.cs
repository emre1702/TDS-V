using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Add_WeaponStats : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerWeaponBodypartStats");

            migrationBuilder.DropTable(
                name: "PlayerWeaponStats");

            migrationBuilder.DropColumn(
                name: "DamageExpMult",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "HeadshotsExpMult",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "HitsExpMult",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "KillsExpMult",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "ShotsExpMult",
                table: "Weapons");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:ped_body_part", "head,foot,arm,leg,upper_body,lower_body,back,hand,neck");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:ped_body_part", "head,foot,arm,leg,upper_body,lower_body,back,hand,neck");

            migrationBuilder.AddColumn<float>(
                name: "DamageExpMult",
                table: "Weapons",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HeadshotsExpMult",
                table: "Weapons",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HitsExpMult",
                table: "Weapons",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "KillsExpMult",
                table: "Weapons",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ShotsExpMult",
                table: "Weapons",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "PlayerWeaponBodypartStats",
                columns: table => new
                {
                    BodyPart = table.Column<PedBodyPart>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    WeaponHash = table.Column<WeaponHash>(nullable: false),
                    AmountHits = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountOfficialHits = table.Column<int>(nullable: false, defaultValue: 0),
                    DealtDamage = table.Column<long>(nullable: false, defaultValue: 0L),
                    DealtOfficialDamage = table.Column<long>(nullable: false, defaultValue: 0L),
                    Kills = table.Column<int>(nullable: false, defaultValue: 0),
                    OfficialKills = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerWeaponBodypartStats", x => new { x.PlayerId, x.WeaponHash, x.BodyPart });
                    table.ForeignKey(
                        name: "FK_PlayerWeaponBodypartStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerWeaponBodypartStats_Weapons_WeaponHash",
                        column: x => x.WeaponHash,
                        principalTable: "Weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerWeaponStats",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    WeaponHash = table.Column<WeaponHash>(nullable: false),
                    AmountHeadshots = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountHits = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountOfficialHeadshots = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountOfficialHits = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountOfficialShots = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountShots = table.Column<int>(nullable: false, defaultValue: 0),
                    DealtDamage = table.Column<long>(nullable: false, defaultValue: 0L),
                    DealtOfficialDamage = table.Column<long>(nullable: false, defaultValue: 0L),
                    Kills = table.Column<int>(nullable: false, defaultValue: 0),
                    OfficialKills = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerWeaponStats", x => new { x.PlayerId, x.WeaponHash });
                    table.ForeignKey(
                        name: "FK_PlayerWeaponStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerWeaponStats_Weapons_WeaponHash",
                        column: x => x.WeaponHash,
                        principalTable: "Weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Sniperrifle,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 101f, 0.005f, 1000f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Fireextinguisher,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Compactlauncher,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 100f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Snowball,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 10f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Vintagepistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 34f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatpdw,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 28f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Heavysniper_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 216f, 0.005f, 2f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Heavysniper,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 216f, 0.005f, 2f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Autoshotgun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 162f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Microsmg,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 21f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Wrench,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 40f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 26f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pumpshotgun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 58f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Appistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 28f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Ball,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Molotov,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 10f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.CeramicPistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 20f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Smg,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 22f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Stickybomb,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 100f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Petrolcan,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Stungun,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Stone_hatchet,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 50f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultrifle_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 30f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Heavyshotgun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 117f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Minigun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 30f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Golfclub,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 40f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Raycarbine,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 23f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Flaregun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 50f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Flare,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Grenadelauncher_smoke,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Hammer,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 40f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pumpshotgun_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 58f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatpistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 27f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Gusenberg,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 34f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Compactrifle,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 34f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Hominglauncher,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 150f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Nightstick,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 35f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Marksmanrifle_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 65f, 0.005f, 2f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Railgun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 50f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Sawnoffshotgun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 160f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Smg_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 22f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bullpuprifle,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 32f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Firework,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 100f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatmg,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 28f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Carbinerifle,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 32f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Crowbar,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 40f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bullpuprifle_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 32f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Snspistol_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 28f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Flashlight,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 30f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Proximine,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 100f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.NavyRevolver,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 40f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Dagger,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 45f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Grenade,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 100f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Poolcue,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 40f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bat,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 40f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Specialcarbine_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 32f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Doubleaction,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 110f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pistol50,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 51f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Knife,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 45f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Mg,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 40f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bullpupshotgun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 112f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bzgas,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Unarmed,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 15f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Grenadelauncher,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 100f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Musket,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 165f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Advancedrifle,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 30f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Raypistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 80f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Rpg,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 100f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Rayminigun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 32f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pipebomb,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 100f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.HazardCan,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Minismg,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 22f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Snspistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 28f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pistol_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 26f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultrifle,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 30f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Specialcarbine,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 32f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Revolver,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 110f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Marksmanrifle,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 65f, 0.005f, 2f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Revolver_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 110f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Battleaxe,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 50f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Heavypistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 40f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Knuckle,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 30f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Machinepistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 20f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatmg_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 28f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Marksmanpistol,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 150f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Machete,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 45f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Switchblade,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 50f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultshotgun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 192f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Dbshotgun,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 166f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultsmg,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 23f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Hatchet,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 50f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bottle,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 10f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Carbinerifle_mk2,
                columns: new[] { "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 32f, 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Parachute,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Smokegrenade,
                columns: new[] { "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult" },
                values: new object[] { 0.005f, 1f, 0.5f, 1f });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerWeaponBodypartStats_WeaponHash",
                table: "PlayerWeaponBodypartStats",
                column: "WeaponHash");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerWeaponStats_WeaponHash",
                table: "PlayerWeaponStats",
                column: "WeaponHash");
        }

        #endregion Protected Methods
    }
}
