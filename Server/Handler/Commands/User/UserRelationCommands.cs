using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Extensions;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Commands.User
{
    public class UserRelationCommands
    {
        [TDSCommand(UserCommand.BlockUser)]
        public async Task BlockUser(ITDSPlayer player, ITDSPlayer target)
        {
            if (player.Entity is null || target.Entity is null)
                return;

            bool continuue = await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                var relation = await dbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id).ConfigureAwait(false);

                if (relation != null && relation.Relation == PlayerRelation.Block)
                {
                    //player.SendChatMessage(string.Format(player.Language.TARGET_ALREADY_BLOCKED, target.DisplayName));
                    await UnblockUser(player, target).ConfigureAwait(false);
                    return false;
                }

                string msg;
                if (relation != null && relation.Relation == PlayerRelation.Friend)
                {
                    msg = string.Format(player.Language.TARGET_REMOVED_FRIEND_ADDED_BLOCK, target.DisplayName);
                }
                else
                {
                    relation = new PlayerRelations { PlayerId = player.Entity.Id, TargetId = target.Entity.Id };
                    dbContext.PlayerRelations.Add(relation);
                    msg = string.Format(player.Language.TARGET_ADDED_BLOCK, target.DisplayName);
                }
                player.Relations.SetRelation(target, PlayerRelation.Block);
                relation.Relation = PlayerRelation.Block;
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                NAPI.Task.RunSafe(() => player.SendChatMessage(msg));

                return true;
            }).ConfigureAwait(false);

            if (!continuue)
                return;

            NAPI.Task.RunSafe(() =>
            {
                if (player.InPrivateChatWith == target)
                    player.Chat.ClosePrivateChat(false);
                target.Voice.SetVoiceTo(player, false);

                target.SendChatMessage(string.Format(target.Language.YOU_GOT_BLOCKED_BY, player.DisplayName));
            });
        }

        [TDSCommand(UserCommand.UnblockUser)]
        public async Task UnblockUser(ITDSPlayer player, ITDSPlayer target)
        {
            if (player.Entity is null || target.Entity is null)
                return;

            await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                var relation = await dbContext.PlayerRelations.FindAsync(player.Entity.Id, target.Entity.Id).ConfigureAwait(false);
                if (relation is null || relation.Relation != PlayerRelation.Block)
                {
                    NAPI.Task.RunSafe(() => player.SendChatMessage(string.Format(player.Language.TARGET_NOT_BLOCKED, target.DisplayName)));
                    return;
                }

                dbContext.PlayerRelations.Remove(relation);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            player.Relations.SetRelation(target, PlayerRelation.None);

            NAPI.Task.RunSafe(() =>
            {
                if (target.Team == player.Team)
                    target.Voice.SetVoiceTo(player, true);

                player.SendChatMessage(string.Format(player.Language.YOU_UNBLOCKED, target.DisplayName));
                target.SendChatMessage(string.Format(target.Language.YOU_GOT_UNBLOCKED_BY, player.DisplayName));
            });
        }
    }
}
