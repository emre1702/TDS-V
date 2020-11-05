using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Command;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.SeedData.Command
{
    public static class CommandInfosSeeds
    {
        public static ModelBuilder HasCommandInfos(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommandInfos>().HasData(
                new CommandInfos { Id = 1, Language = Language.German, Info = "Schreibt öffentlich als ein Admin." },
                new CommandInfos { Id = 1, Language = Language.English, Info = "Writes public as an admin." },
                new CommandInfos { Id = 2, Language = Language.German, Info = "Schreibt intern nur den Admins." },
                new CommandInfos { Id = 2, Language = Language.English, Info = "Writes internally to admins only." },
                new CommandInfos { Id = 3, Language = Language.German, Info = "Bannt einen Spieler vom gesamten Server." },
                new CommandInfos { Id = 3, Language = Language.English, Info = "Bans a player out of the server." },
                new CommandInfos { Id = 4, Language = Language.German, Info = "Teleportiert den Nutzer zu einem Spieler (evtl. in sein Auto) oder zu den angegebenen Koordinaten." },
                new CommandInfos { Id = 4, Language = Language.English, Info = "Warps the user to another player (maybe in his vehicle) or to the defined coordinates." },
                new CommandInfos { Id = 5, Language = Language.German, Info = "Kickt einen Spieler vom Server." },
                new CommandInfos { Id = 5, Language = Language.English, Info = "Kicks a player out of the server." },
                new CommandInfos { Id = 6, Language = Language.German, Info = "Bannt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." },
                new CommandInfos { Id = 6, Language = Language.English, Info = "Bans a player out of the lobby in which the command was used." },
                new CommandInfos { Id = 7, Language = Language.German, Info = "Kickt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." },
                new CommandInfos { Id = 7, Language = Language.English, Info = "Kicks a player out of the lobby in which the command was used." },
                new CommandInfos { Id = 8, Language = Language.German, Info = "Mutet einen Spieler im normalen Chat." },
                new CommandInfos { Id = 8, Language = Language.English, Info = "Mutes a player in the normal chat." },
                new CommandInfos { Id = 9, Language = Language.German, Info = "Beendet die jetzige Runde in der jeweiligen Lobby." },
                new CommandInfos { Id = 9, Language = Language.English, Info = "Ends the current round in the lobby." },
                new CommandInfos { Id = 10, Language = Language.German, Info = "Verlässt die jetzige Lobby." },
                new CommandInfos { Id = 10, Language = Language.English, Info = "Leaves the current lobby." },
                new CommandInfos { Id = 11, Language = Language.German, Info = "Tötet den Nutzer (Selbstmord)." },
                new CommandInfos { Id = 11, Language = Language.English, Info = "Kills the user (suicide)." },
                new CommandInfos { Id = 12, Language = Language.German, Info = "Globaler Chat, welcher überall gelesen werden kann." },
                new CommandInfos { Id = 12, Language = Language.English, Info = "Global chat which can be read everywhere." },
                new CommandInfos { Id = 13, Language = Language.German, Info = "Sendet die Nachricht nur zum eigenen Team." },
                new CommandInfos { Id = 13, Language = Language.English, Info = "Sends the message to the current team only." },
                new CommandInfos { Id = 14, Language = Language.German, Info = "Gibt die Position des Spielers aus." },
                new CommandInfos { Id = 14, Language = Language.English, Info = "Outputs the position of the player." },
                new CommandInfos { Id = 15, Language = Language.German, Info = "Sendet eine Nachricht im Privatchat." },
                new CommandInfos { Id = 15, Language = Language.English, Info = "Sends a message in private chat." },
                new CommandInfos { Id = 16, Language = Language.German, Info = "Schließt den Privatchat oder nimmt eine Privatchat-Anfrage zurück." },
                new CommandInfos { Id = 16, Language = Language.English, Info = "Closes a private chat or withdraws a private chat request." },
                new CommandInfos { Id = 17, Language = Language.German, Info = "Sendet eine Anfrage für einen Privatchat oder nimmt die Anfrage eines Users an." },
                new CommandInfos { Id = 17, Language = Language.English, Info = "Sends a private chat request or accepts the request of another user." },
                new CommandInfos { Id = 18, Language = Language.German, Info = "Private Nachricht an einen bestimmten Spieler." },
                new CommandInfos { Id = 18, Language = Language.English, Info = "Private message to a specific player." },
                new CommandInfos { Id = 19, Language = Language.German, Info = "Gibt dir deine User-Id aus." },
                new CommandInfos { Id = 19, Language = Language.English, Info = "Outputs your user-id to yourself." },
                new CommandInfos { Id = 20, Language = Language.German, Info = "Fügt das Ziel in deine Blocklist ein, sodass du keine Nachrichten mehr von ihm liest, er dich nicht einladen kann usw." },
                new CommandInfos { Id = 20, Language = Language.English, Info = "Adds the target into your blocklist so you won't see messages from him, he can't invite you anymore etc." },
                new CommandInfos { Id = 21, Language = Language.German, Info = "Entfernt das Ziel aus der Blockliste." },
                new CommandInfos { Id = 21, Language = Language.English, Info = "Removes the target from the blocklist." },
                new CommandInfos { Id = 23, Language = Language.German, Info = "Mutet einen Spieler im Voice-Chat." },
                new CommandInfos { Id = 23, Language = Language.English, Info = "Mutes a player in the voice-chat." },
                new CommandInfos { Id = 24, Language = Language.German, Info = "Gibt einem Spieler Geld." },
                new CommandInfos { Id = 24, Language = Language.English, Info = "Gives money to a player." },
                new CommandInfos { Id = 25, Language = Language.German, Info = "Ladet einen Spieler in die eigene Lobby ein (falls möglich)." },
                new CommandInfos { Id = 25, Language = Language.English, Info = "Invites a player to your lobby (if possible)." },
                new CommandInfos { Id = 26, Language = Language.German, Info = "Befehl zum schnellen Testen von Codes." },
                new CommandInfos { Id = 26, Language = Language.English, Info = "Command for quick testing of codes." },
                new CommandInfos { Id = 27, Language = Language.German, Info = "Erstellt ein Haus in der Gang-Lobby." },
                new CommandInfos { Id = 27, Language = Language.English, Info = "Creates a house in the gang lobby." }
            );
            return modelBuilder;
        }
    }
}
