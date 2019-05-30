using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;
using Votebot.Services;

namespace Votebot.Commands
{
    public class ToggleMultiVote : ModuleBase<SocketCommandContext>
    {
        public VoteControllerManager VoteControllerManager { get; set; }

        private const string TRUE_MESSAGE = "Multiple votes are now allowed.";
        private const string FALSE_MESSAGE = "Multiple votes are now disallowed.";
        private const string ERROR_MESSAGE = "Input not recognised. Try yes/no.";


        [Command("MultiVote"), Alias("mv"), Summary("Set whether users can vote for multiple options.")]
        public async Task Toggle(string input)
        {
            Context.Message.DeleteAsync();
            VoteController vc = VoteControllerManager.GetVoteController(Context.Channel);
            input = input.ToUpper();

            if (new[] { "YES", "TRUE", "Y", "T" }.Contains(input))
            {
                vc.MultipleVotesAllowed = true;
                await Context.Channel.SendFileAsync(TRUE_MESSAGE);
            }
            else if (new[] { "NO", "FALSE", "N", "F" }.Contains(input))
            {
                vc.MultipleVotesAllowed = false;
                await Context.Channel.SendFileAsync(FALSE_MESSAGE);
            }
            else
            {
                await Context.Channel.SendFileAsync(ERROR_MESSAGE);
            }
        }

        [Command("MultiVote"), Alias("mv"), Summary("Turn on voting for multiple options.")]
        public async Task Toggle()
        {
            Context.Message.DeleteAsync();
            VoteController vc = VoteControllerManager.GetVoteController(Context.Channel);
            vc.MultipleVotesAllowed = true;
            await Context.Channel.SendFileAsync(TRUE_MESSAGE);
        }

        [Command("GetMultiVote"), Summary("See if voting for multiple options is allowed.")]
        public async Task GetMultiVote()
        {
            Context.Message.DeleteAsync();
            VoteController vc = VoteControllerManager.GetVoteController(Context.Channel);
            await Context.Channel.SendFileAsync(vc.MultipleVotesAllowed ? "Yes" : "No");
        }
    }
}
