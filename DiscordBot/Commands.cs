using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DiscordBot {
	public class Commands {

		[Command("hi")]
		public async Task GreetPlayer ( CommandContext ctx ) {
			await ctx.RespondAsync ( $"👋 Hello {ctx.User.Mention}!" );
		}
	}
}
