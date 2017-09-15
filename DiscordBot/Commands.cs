using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Audio.Streams;
using Discord.Commands;

namespace DiscordBot {
	public class UserCommands : ModuleBase {

		private Random random = new Random ();

		[Command("hi"), Summary("Greets the user"), Alias("hello", "hallo", "greet")]
		public async Task GreetUser ( params string[] args ) {
			await ReplyAsync ( $":wave: Hello {this.Context.User.Mention} :wave:" );
		}

		[Command("8ball")]
		public async Task EightBall ( params string[] args ) {
			int rndm = random.Next ( 1, 16 );

			if ( rndm >= 1 && rndm <= 3 )
				await ReplyAsync ( "Yes" );
			else if ( rndm == 4 )
				await ReplyAsync ( "Absolutely" );
			else if ( rndm >= 5 && rndm <= 7 )
				await ReplyAsync ( "No" );
			else if ( rndm == 8 )
				await ReplyAsync ( "Never" );
			else if ( rndm >= 9 && rndm <= 11 )
				await ReplyAsync ( "Maybe" );
			else if ( rndm == 12 )
				await ReplyAsync ( "Ask again" );
			else if ( rndm == 13 )
				await ReplyAsync ( "I don't know" );
			else if ( rndm == 14 )
				await ReplyAsync ( "Not sure" );
			else if ( rndm == 15 )
				await ReplyAsync ( "You should ask him" );
		}
	}

	public class MusicCommands : ModuleBase {

		[Command ( "volume" ), Summary ( "Sets the volume of the music." ), Alias ( "setvolume", "vol" )]
		public async Task SetSoundVolume ( string volume ) {
			Program.clientsVolume[this.Context.Client] = float.Parse ( volume, CultureInfo.InvariantCulture );
		}

		[Command("join"), Summary("Joins your channel"), Alias ("come")]
		public async Task ComeToChannel ( params string[] args ) {
			IVoiceChannel channel = ( this.Context.User as IGuildUser )?.VoiceChannel;
			if ( channel == null ) {
				await this.Context.Channel.SendMessageAsync ( "You are not in a voice channel!" );
				return;
			}
			IAudioClient audioclient = await channel.ConnectAsync ();
			Program.clientsAudioClient[this.Context.Client] = audioclient;
		} 

		private Process CreateStream ( string url, float volume ) {

			string volstr = volume.ToString ().Replace ( ',', '.' );

			return Process.Start ( new ProcessStartInfo {
				FileName = "bash",
				Arguments = "-c \"/usr/local/bin/youtube-dl -o - " + url+ " | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 -af volume="+ volstr + " pipe:1\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			} );
		}


		[Command ("play"), Summary("Plays a youtube-link"), Alias ( "yt", "youtube")]
		public async Task PlayYoutube ( string url ) {
			IVoiceChannel channel = ( this.Context.User as IVoiceState ).VoiceChannel;

			IAudioClient client;

			if ( Program.clientsAudioClient.ContainsKey ( this.Context.Client ) )
				client = Program.clientsAudioClient[this.Context.Client];
			else {
				client = await channel.ConnectAsync ();
				Program.clientsAudioClient[this.Context.Client] = client;
			}

			if ( !Program.clientsVolume.ContainsKey ( this.Context.Client ) )
				Program.clientsVolume[this.Context.Client] = 0.1f;

			Process ffmpeg = CreateStream ( url, Program.clientsVolume[this.Context.Client] );
			Stream output = ffmpeg.StandardOutput.BaseStream;
			AudioOutStream discord = client.CreatePCMStream ( AudioApplication.Music );
			await output.CopyToAsync ( discord );
			await discord.FlushAsync ().ConfigureAwait ( false );
		}

		/* [Command ( "stream" ), Summary ( "Plays a stream" ), Alias ( "radio" )]
		public async Task PlayStream ( string url ) {
			IVoiceChannel channel = ( this.Context.User as IVoiceState ).VoiceChannel;

			IAudioClient client;

			if ( Program.clientsAudioClient.ContainsKey ( this.Context.Client ) )
				client = Program.clientsAudioClient[this.Context.Client];
			else {
				client = await channel.ConnectAsync ();
				Program.clientsAudioClient[this.Context.Client] = client;
			}
			Process ffmpeg = CreateStream ( url, this.Context.Channel );
			Stream output = ffmpeg.StandardOutput.BaseStream;
			AudioOutStream discord = client.CreatePCMStream ( AudioApplication.Music );

			byte[] soundbyte = await GetByteByStream ( output );

			await output.CopyToAsync ( discord );
			await discord.FlushAsync ();
		} */

		
	}

	public class AdminCommands : ModuleBase {

	}
}
