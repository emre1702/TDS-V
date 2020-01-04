namespace TDS_Common.Enum.Challenge
{
    public enum EChallengeType
    {
        Kills,
        Assists,
        Damage,
        PlayTime,
        RoundPlayed,
        BombDefuse,
        BombPlant,
        Killstreak,

        BuyMaps,                // Buy {0} maps.
        ReviewMaps,             // Review {0} maps.

        ReadTheRules,           // Read the rules atleast once.
        ReadTheFAQ,             // Read the FAQ atleast once.
        ChangeSettings,         // Change any userpanel setting (except Discord Identity)
        JoinDiscordServer,      // Join the Discord server and set your Discord identity in the userpanel settings.

        WriteHelpfulIssue,      // Write a helpful issue in GitHub.

        CreatorOfAcceptedMap,   // Create a map that passes the testing phase
        BeHelpfulEnough,        // Help the server enough to get the "Contributor" role.

        // HeadshotKills,
        // JoinActions,
        // JoinOrCreateGang,       // Join or create a gang.
    }
}
