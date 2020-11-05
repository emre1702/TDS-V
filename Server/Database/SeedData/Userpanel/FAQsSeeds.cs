using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDS_Server.Database.Entity.Userpanel;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.SeedData.Userpanel
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
                }
            );
            return modelBuilder;
        }
    }
}
