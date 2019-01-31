using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Votebot.Controllers;

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
                .AddSingleton<VoteController>()
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
    }
}
