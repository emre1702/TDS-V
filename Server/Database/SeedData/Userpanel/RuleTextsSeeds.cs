using Microsoft.EntityFrameworkCore;
using TDS_Server.Database.Entity.Userpanel;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.SeedData.Userpanel
{
    public static class RuleTextsSeeds
    {
        public static ModelBuilder HasRuleTexts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RuleTexts>().HasData(
                new RuleTexts
                {
                    RuleId = 1,
                    Language = Language.English,
                    RuleStr = @"Teaming with opposing players is strictly forbidden!"
                        + "\nThis means the deliberate sparing, better treatment, letting or similar of certain opposing players without the permission of the own team members."
                        + "\nIf such behaviour is noticed, it can lead to severe penalties and is permanently noted."
                },
                new RuleTexts
                {
                    RuleId = 1,
                    Language = Language.German,
                    RuleStr = @"Teamen mit gegnerischen Spielern ist strengstens verboten!"
                        + "\nDamit ist das absichtliche Verschonen, besser Behandeln, Lassen o.ä. von bestimmten gegnerischen Spielern ohne Erlaubnis der eigenen Team-Mitglieder gemeint."
                        + "\nWird ein solches Verhalten bemerkt, kann es zu starken Strafen führen und es wird permanent notiert."
                },

                new RuleTexts
                {
                    RuleId = 2,
                    Language = Language.English,
                    RuleStr = @"The normal chat in an official lobby has rules, the other chats (private lobbies, dirty) none."
                        + "\nBy 'normal chat' we mean all chat methods (global, team, etc.) in the 'normal' chat area."
                        + "\nThe chat rules listed here are ONLY for the normal chat in an official lobby."
                        + "\nChats in private lobbies can be freely monitored by the lobby owners."
                },
                new RuleTexts
                {
                    RuleId = 2,
                    Language = Language.German,
                    RuleStr = "Der normale Chat in einer offiziellen Lobby hat Regeln, die restlichen Chats (private Lobbys, dirty) jedoch keine."
                        + "\nUnter 'normaler Chat' versteht man alle Chats-Methode (global, team usw.) im 'normal' Chat-Bereich."
                        + "\nDie hier aufgelisteten Chat-Regeln richten sich NUR an den normalen Chat in einer offiziellen Lobby."
                        + "\nChats in privaten Lobbys können frei von den Lobby-Besitzern überwacht werden."
                },

                new RuleTexts
                {
                    RuleId = 3,
                    Language = Language.English,
                    RuleStr = "Admins have to follow the same rules as players do."
                },
                new RuleTexts
                {
                    RuleId = 3,
                    Language = Language.German,
                    RuleStr = "Admins haben genauso die Regeln zu befolgen wie auch die Spieler."
                },

                new RuleTexts
                {
                    RuleId = 4,
                    Language = Language.English,
                    RuleStr = "Exploitation of the commands is strictly forbidden!"
                        + "\nAdmin commands for 'punishing' (kick, mute, ban etc.) may only be used for violations of rules."
                },
                new RuleTexts
                {
                    RuleId = 4,
                    Language = Language.German,
                    RuleStr = "Ausnutzung der Befehle ist strengstens verboten!"
                        + "\nAdmin-Befehle zum 'Bestrafen' (Kick, Mute, Ban usw.) dürfen auch nur bei Verstößen gegen Regeln genutzt werden."
                },

                new RuleTexts
                {
                    RuleId = 5,
                    Language = Language.English,
                    RuleStr = "If you are not sure if the time for e.g. Mute or Bann could be too high,"
                        + "\nask your team leader - if you can't reach someone quickly, choose a lower time."
                        + "\nToo high times are bad, too low times are no problem."
                },
                new RuleTexts
                {
                    RuleId = 5,
                    Language = Language.German,
                    RuleStr = "Wenn du dir nicht sicher bist, ob die Zeit für z.B. Mute oder Bann zu hoch sein könnte,"
                        + "\nfrage deinen Team-Leiter - kannst du niemanden auf die Schnelle erreichen, so entscheide dich für eine niedrigere Zeit."
                        + "\nZu hohe Zeiten sind schlecht, zu niedrige kein Problem."
                },

                new RuleTexts
                {
                    RuleId = 6,
                    Language = Language.English,
                    RuleStr = "All admin rules with the exception of activity duty are also valid for VIPs."
                },
                new RuleTexts
                {
                    RuleId = 6,
                    Language = Language.German,
                    RuleStr = "Alle Admin-Regeln mit Ausnahme von Aktivitäts-Pflicht sind auch gültig für VIPs."
                },

                new RuleTexts
                {
                    RuleId = 7,
                    Language = Language.English,
                    RuleStr = "The VIPs are free to decide whether they want to use their rights or not."
                },
                new RuleTexts
                {
                    RuleId = 7,
                    Language = Language.German,
                    RuleStr = "Den VIPs ist es frei überlassen, ob sie ihre Rechte nutzen wollen oder nicht."
                }
            );
            return modelBuilder;
        }
    }
}
