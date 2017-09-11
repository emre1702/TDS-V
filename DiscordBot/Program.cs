using System.Threading.Tasks;
using DSharpPlus;

namespace DiscordBot {
	class Program {
		static DiscordClient client;

		static void Main ( string[] args ) {
			MainAsync ( args ).ConfigureAwait ( false ).GetAwaiter ().GetResult ();
		}

		static async Task MainAsync ( string[] args ) {

			client = new DiscordClient ( new DiscordConfiguration {
				Token = "MzU2NTc4NTE1NDcyMDg5MDg5.DJdZLg.eGrl3o54bdpyWIexxfnvIw3z_VI",
				TokenType = TokenType.Bot,
				AutoReconnect = true
			} );

			client.MessageCreated += async ( e ) => {
				if ( e.Message.Content.ToLower ().StartsWith ( "hi" ) )
					await e.Message.RespondAsync ( "Hello!" );
			};

			await client.ConnectAsync ();
			await Task.Delay ( -1 );
		}
	}
}
