using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS.Server.Database.Entity.Userpanel;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.SeedData.Userpanel
{
    public static class FAQsSeeds
    {
        public static ModelBuilder HasFAQs(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FAQs>().HasData(
                new FAQs
                {
                    Id = 1,
                    Language = Language.English,
                    Question = "How do I activate my cursor?",
                    Answer = "With the END key on your keyboard."
                },
                new FAQs
                {
                    Id = 1,
                    Language = Language.German,
                    Question = "Wie aktiviere ich meinen Cursor?",
                    Answer = "Mit der ENDE Taste auf deiner Tastatur."
                },

                new FAQs
                {
                    Id = 2,
                    Language = Language.English,
                    Question = "What is the 'Allow data transfer' setting in the userpanel?",
                    Answer = "In case of a transfer of TDS-V, the database will also be transferred, but without the player data (for data protection reasons)."
                            + "\nHowever, if you want to keep your data, you must allow it in the user panel."
                            + "\nThe data does not contain any sensitive information - IPs are not stored, passwords are secure (hash + salt)."
                },
                new FAQs
                {
                    Id = 2,
                    Language = Language.German,
                    Question = "Was ist die 'Erlaube Daten-Transfer' Einstellung im Userpanel?",
                    Answer = "Im Falle einer Übergabe von TDS-V wird die Datenbank auch übergeben, jedoch ohne die Spieler-Daten (aus Datenschutz-Gründen)."
                            + "\nFalls du jedoch deine Daten auch dann weiterhin behalten willst, musst du es im Userpanel erlauben."
                            + "\nDie Daten beinhalten keine sensiblen Informationen - IPs werden nicht gespeichert, Passwörter sind sicher (Hash + Salt)."
                },

                new FAQs
                {
                    Id = 3,
                    Language = Language.English,
                    Question = "What are the rewards for created maps?",
                    Answer = "The map creator gets following rewards (only official lobbies):"
                            + "\n$1 - map got selected randomly"
                            + "\n$5 - map got voted"
                            + "\n$15 - map got bought"
                },
                new FAQs
                {
                    Id = 3,
                    Language = Language.German,
                    Question = "Was sind die Belohnungen für erstelle Karten?",
                    Answer = "Der Karten-Ersteller bekommt folgende Belohnungen (nur in offiziellen Lobbies):"
                            + "\n$1 - Karte wurde zufällig ausgewählt"
                            + "\n$5 - Karte wurde per Abstimmung gewählt"
                            + "\n$15 - Karte wurde gekauft"
                },

                new FAQs
                {
                    Id = 4,
                    Language = Language.English,
                    Question = "Why are the shirts in e.g. Arena differently colored?",
                    Answer = "The color of the shirts gives you these two information:"
                            + "\n1. Color of the sleeves: Enemy (red) or friend (green)"
                            + "\n2. Rest: Team color"
                },
                new FAQs
                {
                    Id = 4,
                    Language = Language.German,
                    Question = "Warum sind die Shirts in z.B. Arena verschieden farbig?",
                    Answer = "Die Farbe der Shirts gibt dir diese zwei Informationen:"
                            + "\n1. Farbe der Ärmel: Feind (rot) oder Freund (grün)"
                            + "\n2. Rest: Team-Farbe"
                }

            );
            return modelBuilder;
        }
    }
}