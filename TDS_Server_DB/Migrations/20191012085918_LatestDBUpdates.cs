using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class LatestDBUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowNametagOnlyOnAiming",
                table: "server_settings",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<float>(
                name: "MinMapRatingForNewMaps",
                table: "server_settings",
                nullable: false,
                defaultValue: 3f);

            migrationBuilder.AlterColumn<bool>(
                name: "Hitsound",
                table: "player_settings",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "FloatingDamageInfo",
                table: "player_settings",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Bloodscreen",
                table: "player_settings",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 2, ELanguage.German },
                column: "Answer",
                value: @"Im Falle einer Übergabe von TDS-V wird die Datenbank auch übergeben, jedoch ohne die Spieler-Daten (aus Datenschutz-Gründen).
Falls du jedoch deine Daten auch dann weiterhin behalten willst, musst du es im Userpanel erlauben.
Die Daten beinhalten keine sensiblen Informationen - IPs werden nicht gespeichert, Passwörter sind sicher (Hash + Salt).");

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 2, ELanguage.English },
                column: "Answer",
                value: @"In case of a transfer of TDS-V, the database will also be transferred, but without the player data (for data protection reasons).
However, if you want to keep your data, you must allow it in the user panel.
The data does not contain any sensitive information - IPs are not stored, passwords are secure (hash + salt).");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 1, ELanguage.German },
                column: "RuleStr",
                value: @"Teamen mit gegnerischen Spielern ist strengstens verboten!
Damit ist das absichtliche Verschonen, besser Behandeln, Lassen o.ä. von bestimmten gegnerischen Spielern ohne Erlaubnis der eigenen Team-Mitglieder gemeint.
Wird ein solches Verhalten bemerkt, kann es zu starken Strafen führen und es wird permanent notiert.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 1, ELanguage.English },
                column: "RuleStr",
                value: @"Teaming with opposing players is strictly forbidden!
This means the deliberate sparing, better treatment, letting or similar of certain opposing players without the permission of the own team members.
If such behaviour is noticed, it can lead to severe penalties and is permanently noted.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 2, ELanguage.German },
                column: "RuleStr",
                value: @"Der normale Chat in einer offiziellen Lobby hat Regeln, die restlichen Chats (private Lobbys, dirty) jedoch keine.
Unter 'normaler Chat' versteht man alle Chats-Methode (global, team usw.) im 'normal' Chat-Bereich.
Die hier aufgelisteten Chat-Regeln richten sich NUR an den normalen Chat in einer offiziellen Lobby.
Chats in privaten Lobbys können frei von den Lobby-Besitzern überwacht werden.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 2, ELanguage.English },
                column: "RuleStr",
                value: @"The normal chat in an official lobby has rules, the other chats (private lobbies, dirty) none.
By 'normal chat' we mean all chat methods (global, team, etc.) in the 'normal' chat area.
The chat rules listed here are ONLY for the normal chat in an official lobby.
Chats in private lobbies can be freely monitored by the lobby owners.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 4, ELanguage.German },
                column: "RuleStr",
                value: @"Ausnutzung der Befehle ist strengstens verboten!
Admin-Befehle zum 'Bestrafen' (Kick, Mute, Ban usw.) dürfen auch nur bei Verstößen gegen Regeln genutzt werden.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 4, ELanguage.English },
                column: "RuleStr",
                value: @"Exploitation of the commands is strictly forbidden!
Admin commands for 'punishing' (kick, mute, ban etc.) may only be used for violations of rules.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 5, ELanguage.German },
                column: "RuleStr",
                value: @"Wenn du dir nicht sicher bist, ob die Zeit für z.B. Mute oder Bann zu hoch sein könnte,
frage deinen Team-Leiter - kannst du niemanden auf die Schnelle erreichen, so entscheide dich für eine niedrigere Zeit.
Zu hohe Zeiten sind schlecht, zu niedrige kein Problem.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 5, ELanguage.English },
                column: "RuleStr",
                value: @"If you are not sure if the time for e.g. Mute or Bann could be too high,
ask your team leader - if you can't reach someone quickly, choose a lower time.
Too high times are bad, too low times are no problem.");

            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "ID",
                keyValue: (short)1,
                column: "MinMapRatingForNewMaps",
                value: 3f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinMapRatingForNewMaps",
                table: "server_settings");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowNametagOnlyOnAiming",
                table: "server_settings",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "Hitsound",
                table: "player_settings",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "FloatingDamageInfo",
                table: "player_settings",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "Bloodscreen",
                table: "player_settings",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 2, ELanguage.German },
                column: "Answer",
                value: @"Im Falle einer Übergabe von TDS-V wird die Datenbank auch übergeben, jedoch ohne die Spieler-Daten (aus Datenschutz-Gründen).
Falls du jedoch deine Daten auch dann weiterhin behalten willst, musst du es im Userpanel erlauben.
Die Daten beinhalten keine sensiblen Informationen - IPs werden nicht gespeichert, Passwörter sind sicher (Hash + Salt).");

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 2, ELanguage.English },
                column: "Answer",
                value: @"In case of a transfer of TDS-V, the database will also be transferred, but without the player data (for data protection reasons).
However, if you want to keep your data, you must allow it in the user panel.
The data does not contain any sensitive information - IPs are not stored, passwords are secure (hash + salt).");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 1, ELanguage.German },
                column: "RuleStr",
                value: @"Teamen mit gegnerischen Spielern ist strengstens verboten!
Damit ist das absichtliche Verschonen, besser Behandeln, Lassen o.ä. von bestimmten gegnerischen Spielern ohne Erlaubnis der eigenen Team-Mitglieder gemeint.
Wird ein solches Verhalten bemerkt, kann es zu starken Strafen führen und es wird permanent notiert.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 1, ELanguage.English },
                column: "RuleStr",
                value: @"Teaming with opposing players is strictly forbidden!
This means the deliberate sparing, better treatment, letting or similar of certain opposing players without the permission of the own team members.
If such behaviour is noticed, it can lead to severe penalties and is permanently noted.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 2, ELanguage.German },
                column: "RuleStr",
                value: @"Der normale Chat in einer offiziellen Lobby hat Regeln, die restlichen Chats (private Lobbys, dirty) jedoch keine.
Unter 'normaler Chat' versteht man alle Chats-Methode (global, team usw.) im 'normal' Chat-Bereich.
Die hier aufgelisteten Chat-Regeln richten sich NUR an den normalen Chat in einer offiziellen Lobby.
Chats in privaten Lobbys können frei von den Lobby-Besitzern überwacht werden.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 2, ELanguage.English },
                column: "RuleStr",
                value: @"The normal chat in an official lobby has rules, the other chats (private lobbies, dirty) none.
By 'normal chat' we mean all chat methods (global, team, etc.) in the 'normal' chat area.
The chat rules listed here are ONLY for the normal chat in an official lobby.
Chats in private lobbies can be freely monitored by the lobby owners.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 4, ELanguage.German },
                column: "RuleStr",
                value: @"Ausnutzung der Befehle ist strengstens verboten!
Admin-Befehle zum 'Bestrafen' (Kick, Mute, Ban usw.) dürfen auch nur bei Verstößen gegen Regeln genutzt werden.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 4, ELanguage.English },
                column: "RuleStr",
                value: @"Exploitation of the commands is strictly forbidden!
Admin commands for 'punishing' (kick, mute, ban etc.) may only be used for violations of rules.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 5, ELanguage.German },
                column: "RuleStr",
                value: @"Wenn du dir nicht sicher bist, ob die Zeit für z.B. Mute oder Bann zu hoch sein könnte,
frage deinen Team-Leiter - kannst du niemanden auf die Schnelle erreichen, so entscheide dich für eine niedrigere Zeit.
Zu hohe Zeiten sind schlecht, zu niedrige kein Problem.");

            migrationBuilder.UpdateData(
                table: "rule_texts",
                keyColumns: new[] { "RuleID", "Language" },
                keyValues: new object[] { 5, ELanguage.English },
                column: "RuleStr",
                value: @"If you are not sure if the time for e.g. Mute or Bann could be too high,
ask your team leader - if you can't reach someone quickly, choose a lower time.
Too high times are bad, too low times are no problem.");
        }
    }
}
