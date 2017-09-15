using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot {
	public partial class Program {
		public static ConcurrentDictionary<IDiscordClient, IAudioClient> clientsAudioClient = new ConcurrentDictionary<IDiscordClient, IAudioClient> ();
		public static ConcurrentDictionary<IDiscordClient, float> clientsVolume = new ConcurrentDictionary<IDiscordClient, float> ();

		private DiscordSocketClient client;
		private CommandService commands;
		private IServiceProvider services;

		public static void Main ( string[] args ) {
			new Program ().MainAsync ().GetAwaiter ().GetResult ();
		}

		public async Task MainAsync () {
			this.client = new DiscordSocketClient ();

			this.client.Log += this.Log;

			this.commands = new CommandService ( new CommandServiceConfig {
				DefaultRunMode = RunMode.Async,
				LogLevel = LogSeverity.Debug
			} );

			this.services = new ServiceCollection ().BuildServiceProvider ();

			await InstallCommands ();

			await this.client.LoginAsync ( TokenType.Bot, "MzU2NTc4NTE1NDcyMDg5MDg5.DJdZLg.eGrl3o54bdpyWIexxfnvIw3z_VI" );
			await this.client.StartAsync ();

			await Task.Delay ( -1 );
		}

		private Task Log ( LogMessage msg ) {
			Console.WriteLine ( msg.ToString () );
			return Task.CompletedTask;
		}

		public async Task InstallCommands ( ) {
			// Hook the MessageReceived Event into our Command Handler
			client.MessageReceived += this.HandleCommand;
			// Discover all of the commands in this assembly and load them.
			await commands.AddModulesAsync ( Assembly.GetEntryAssembly () );
		}

		public async Task HandleCommand ( SocketMessage messageParam ) {
			// Don't process the command if it was a System Message
			SocketUserMessage message = messageParam as SocketUserMessage;
			if ( message == null )
				return;
			// Ignore self when checking commands
			if ( messageParam.Author.IsBot )
				return;     
			// Create a number to track where the prefix ends and the command begins
			int argPos = 0;
			// Determine if the message is a command, based on if it starts with '!' or a mention prefix
			if ( !( message.HasCharPrefix ( '!', ref argPos ) || message.HasMentionPrefix ( client.CurrentUser, ref argPos ) ) )
				return;
			// Create a Command Context
			CommandContext context = new CommandContext ( client, message );
			// Execute the command. (result does not indicate a return value, 
			// rather an object stating if the command executed successfully)
			IResult result = await commands.ExecuteAsync ( context, argPos, this.services );
			if ( !result.IsSuccess )
				await context.Channel.SendMessageAsync ( result.ErrorReason );
		}
	}
}