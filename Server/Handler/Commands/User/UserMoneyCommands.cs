using GTANetworkAPI;
using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Extensions;

namespace TDS_Server.Handler.Commands.User
{
    public class UserMoneyCommands
    {
        private readonly ISettingsHandler _settingsHandler;

        public UserMoneyCommands(ISettingsHandler settingsHandler)
            => _settingsHandler = settingsHandler;

        [TDSCommand(UserCommand.GiveMoney)]
        public void GiveMoney(ITDSPlayer player, ITDSPlayer target, uint money)
        {
            if (player.Entity is null || target.Entity is null)
                return;

            if (money < _settingsHandler.ServerSettings.GiveMoneyMinAmount)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.GIVE_MONEY_TOO_LESS));
                return;
            }

            uint fee = (uint)Math.Ceiling(money * _settingsHandler.ServerSettings.GiveMoneyFee);
            money += fee;

            if (player.Money < money)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.GIVE_MONEY_NEED_FEE, money, fee)));
                return;
            }

            player.MoneyHandler.GiveMoney((int)money * -1);
            target.MoneyHandler.GiveMoney(money - fee);

            NAPI.Task.RunSafe(() =>
            {
                player.SendChatMessage(string.Format(player.Language.YOU_GAVE_MONEY_TO_WITH_FEE, money - fee, fee, target.DisplayName));
                target.SendChatMessage(string.Format(target.Language.YOU_GOT_MONEY_BY_WITH_FEE, money - fee, fee, player.DisplayName));
            });
        }
    }
}
