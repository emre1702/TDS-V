using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace DiscordBot {
	class Program {
		static DiscordClient discord;
		static CommandsNextModule commands;

		static void Main ( string[] args ) {
			MainAsync ( args ).ConfigureAwait ( false ).GetAwaiter ().GetResult ();
		}

		static async Task MainAsync ( string[] args ) {

			discord = new DiscordClient ( new DiscordConfiguration {
				Token = "MzU2NTc4NTE1NDcyMDg5MDg5.DJdZLg.eGrl3o54bdpyWIexxfnvIw3z_VI",
				TokenType = TokenType.Bot,
				AutoReconnect = true,
				UseInternalLogHandler = true,
				LogLevel = LogLevel.Debug
			} );

			commands = discord.UseCommandsNext ( new CommandsNextConfiguration {
				StringPrefix = "!",
				CaseSensitive = false
			} );

			commands.RegisterCommands<Commands> ();

			await discord.ConnectAsync ();
			await Task.Delay ( -1 );
		}

		
	}
}
