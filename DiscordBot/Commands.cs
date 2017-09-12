using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;

namespace DiscordBot {
	public class UserCommands : ModuleBase {

		[Command("hi"), Summary("Greets the user"), Alias("hello", "hallo", "greet")]
		public async Task GreetUser () {
			await ReplyAsync ( $":wave: Hello {this.Context.User.Mention} :wave:" );
		}
	}

	public class MusicCommands : ModuleBase {

		[Command ( "volume" ), Summary ( "Sets the volume of the music." ), Alias ( "setvolume", "vol" )]
		public async Task SetSoundVolume ( string volume ) {
			Program.clientsVolume[this.Context.Client] = float.Parse ( volume, CultureInfo.InvariantCulture );
		}

		[Command("join"), Summary("Joins your channel"), Alias ("come")]
		public async Task ComeToChannel ( ) {
			IVoiceChannel channel = ( this.Context.User as IGuildUser )?.VoiceChannel;
			if ( channel == null ) {
				await this.Context.Channel.SendMessageAsync ( "You are not in a voice channel!" );
				return;
			}
			IAudioClient audioclient = await channel.ConnectAsync ();
			Program.clientsAudioClient[this.Context.Client] = audioclient;
		} 

		private Process CreateStream ( string url, float volume ) {
			Process currentsong = new Process ();

			string volstr = volume.ToString ().Replace ( ',', '.' );
			this.Context.Channel.SendMessageAsync ( volstr );

			currentsong.StartInfo = new ProcessStartInfo {
				FileName = "bash",
				Arguments = "-c \"/usr/local/bin/youtube-dl -o - " + url+ " | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 -af volume="+ volstr + " pipe:1\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			};

			currentsong.Start ();
			return currentsong;
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
			await Task.Delay ( 10000 );
			await discord.FlushAsync ();
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
