using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Votebot.Controllers;
using Votebot.Services;

namespace Votebot
{
    class Program
    {
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        public async Task MainAsync()
        {
            string token = ResourceController.GetToken();

            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 42,
                #if DEBUG
                LogLevel = LogSeverity.Debug
                #else
                LogLevel = LogSeverity.Warning
                #endif
            });

            client.Log += message =>
            {
                Console.WriteLine(message);
                return Task.CompletedTask;
            };

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton<VoteControllerManager>()
                .BuildServiceProvider();

            commands = new CommandService(new CommandServiceConfig
            {
                #if DEBUG
                LogLevel = LogSeverity.Debug
                #else
                LogLevel = LogSeverity.Warning
                #endif
            });

            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            client.MessageReceived += HandleCommandAsync;
            client.ReactionAdded += ReactionAddedHandler;
            client.ReactionRemoved += ReactionRemovedHandler;

            await Task.Delay(-1);
        }

        public async Task HandleCommandAsync(SocketMessage m)
        {
            if (!(m is SocketUserMessage msg)) return;
            if (msg.Author.IsBot) return;

            int argPos = 0;
            if (!msg.HasStringPrefix(ResourceController.GetPrefix(), ref argPos)) return;

            SocketCommandContext context = new SocketCommandContext(client, msg);
            await commands.ExecuteAsync(context, argPos, services);
        }

        private async Task ReactionAddedHandler(Cacheable<IUserMessage, ulong> message,
            ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            VoteControllerManager vcm = (VoteControllerManager) services.GetService(typeof(VoteControllerManager));
            VoteController vc = vcm.GetVoteController(socketMessageChannel);

            await message.DownloadAsync();

            if (vc.CurrentPoll.Options.All(m => m.Id != message.Id))
                return;

            vc.AddVote(reaction);
        }

        private async Task ReactionRemovedHandler(Cacheable<IUserMessage, ulong> message,
            ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            VoteControllerManager vcm = (VoteControllerManager) services.GetService(typeof(VoteControllerManager));
            VoteController vc = vcm.GetVoteController(socketMessageChannel);

            await message.DownloadAsync();

            if (vc.CurrentPoll.Options.All(m => m.Id != message.Id))
                return;

            vc.RemoveVote(reaction);
        }
    }
}
