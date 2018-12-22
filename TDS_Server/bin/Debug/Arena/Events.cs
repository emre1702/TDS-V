using GTANetworkAPI;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class Arena
    {
        public override void OnPlayerEnterColShape(ColShape shape, Character character)
        {
            base.OnPlayerEnterColShape(shape, character);
            if (lobbyBombTakeCol.ContainsKey(this))
            {
                if (character.Lifes > 0 && character.Team == TerroristTeam)
                {
                    TakeBomb(character);
                }
            }
        }
    }
}
