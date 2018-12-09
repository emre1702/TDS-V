namespace TDS.Instance
{

    partial class Damagesys
    {

        /*private static readonly Dictionary<Client, Timer> sDeadTimer = new Dictionary<Client, Timer>();
        public Dictionary<Client, uint> PlayerAssists = new Dictionary<Client, uint>(),
                                        PlayerKills = new Dictionary<Client, uint>();

        //[DisableDefaultOnDeathRespawn] todo: After Version 0.4
        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(Client player, Client killer, uint weapon)
        {
            if (!sDeadTimer.ContainsKey(player))
            {
                Character character = player.GetChar();

                player.Freeze(true);
                sDeadTimer.TryAdd(player, Timer.SetTimer(() => SpawnAfterDeath(character), 2000));

                if (!(character.Lobby is FightLobby lobby))
                    return;

                Damagesys dmgsys = lobby.DmgSys;

                Character killercharacter = dmgsys.GetKiller(character, killer.GetChar());
                killer = killercharacter.Player;

                dmgsys.PlayerSpree.Remove(character);

                if (character.Lifes > 0)
                {
                    lobby.OnPlayerDeath(character, killer, weapon);

                    // Kill //
                    if (killercharacter != character)
                    {
                        if (character.Lobby is Arena)
                            killercharacter.GiveKill();
                        if (!dmgsys.PlayerKills.ContainsKey(killer))
                            dmgsys.PlayerKills.TryAdd(killer, 0);
                        dmgsys.PlayerKills[killer]++;

                        // Killingspree //
                        dmgsys.AddToKillingSpree(killercharacter);
                    }

                    if (character.Lobby is Arena)
                        // Death //
                        character.GiveDeath();

                    // Assist //
                    dmgsys.CheckForAssist(character, killer);
                }
            }
        }

        private Character GetKiller(Character character, Character possiblekillercharacter)
        {
            if (character.Player != possiblekillercharacter.Player && possiblekillercharacter.Player != null && possiblekillercharacter.Player.Exists)
                return possiblekillercharacter;

            Character lasthittercharacter = GetLastHitter(character);
            if (lasthittercharacter != null)
                return lasthittercharacter;

            return character;
        }

        private static void SpawnAfterDeath(Character character)
        {
            sDeadTimer.Remove(character.Player, out Timer timer);
            timer.Kill();
            if (character.Player.Exists)
            {
                NAPI.Player.SpawnPlayer(character.Player, character.Lobby.SpawnPoint, character.Lobby.SpawnRotation.Z);
            }
        }

        private void CheckForAssist(Character character, Client killer)
        {
            if (AllHitters.ContainsKey(character))
            {
                uint halfarmorhp = (lobby.Armor + lobby.Health) / 2;
                foreach (KeyValuePair<Character, int> entry in AllHitters[character])
                {
                    if (entry.Value >= halfarmorhp)
                    {
                        Character targetcharacter = entry.Key;
                        Client target = targetcharacter.Player;
                        if (target.Exists && targetcharacter.Lobby == character.Lobby && killer != target)
                        {
                            if (targetcharacter.Lobby is Arena)
                                targetcharacter.GiveAssist();
                            target.SendLangNotification("got_assist", character.Player.Name);
                            if (!PlayerAssists.ContainsKey(target))
                                PlayerAssists[target] = 0;
                            PlayerAssists[target]++;
                        }
                        if (killer != target ||
                            halfarmorhp % 2 != 0 ||
                            entry.Value != halfarmorhp / 2 ||
                            AllHitters[character].Count > 2)
                            return;
                    }
                }
            }
        }

        public void CheckLastHitter(Character character, out Character lastHitterCharacter)
        {
            if (LastHitterDictionary.ContainsKey(character))
            {
                LastHitterDictionary.Remove(character, out lastHitterCharacter);
                if (lastHitterCharacter.Player.Exists)
                {
                    if (character.Lobby == lastHitterCharacter.Lobby)
                        if (lastHitterCharacter.Lifes > 0)
                        {
                            if (character.Lobby is Arena)
                                lastHitterCharacter.GiveKill();
                            lastHitterCharacter.Player.SendLangNotification("got_last_hitted_kill", character.Player.Name);
                            AddToKillingSpree(lastHitterCharacter);
                        }
                }
            }
            else
                lastHitterCharacter = null;
        }

        public Character GetLastHitter(Character character)
        {
            if (LastHitterDictionary.ContainsKey(character))
            {
                LastHitterDictionary.Remove(character, out Character lasthittercharacter);
                if (lasthittercharacter.Player.Exists)
                {
                    if (character.Lobby == lasthittercharacter.Lobby)
                        return lasthittercharacter;
                }
            }
            return null;
        }*/
    }

}
