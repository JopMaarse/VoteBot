using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Votebot.Controllers;

namespace Votebot.Commands
{
    public class PostHelp : ModuleBase<SocketCommandContext>
    {
        public CommandService Service { get; set; }

        [Command("help"), Alias("h"), Summary("Help command.")]
        public async Task Help()
        {
            Context.Message.DeleteAsync();
            string prefix = ResourceController.GetPrefix();

            EmbedBuilder builder = new EmbedBuilder
            {
                Color = new Color(114, 137, 218),
                Description = "Commands:"
            };

            foreach (ModuleInfo module in Service.Modules)
            {
                foreach (CommandInfo cmd in module.Commands)
                {
                    builder.AddField(x =>
                    {
                        x.Name = string.Join(", ", cmd.Aliases.Select(s => $"{prefix}{s}"));
                        x.Value = cmd.Summary;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("help"), Alias("h"), Summary("Add another command after help to show info on a specific command.")]
        public async Task HelpAsync(string command)
        {
            Context.Message.DeleteAsync();
            string prefix = ResourceController.GetPrefix();

            SearchResult result = Service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"{command} is not a command.");
                return;
            }

            EmbedBuilder builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"Results for {command}:"
            };

            foreach (CommandMatch match in result.Commands)
            {
                CommandInfo cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases.Select(s => $"{prefix}{s}"));
                    x.Value = cmd.Summary;
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}
