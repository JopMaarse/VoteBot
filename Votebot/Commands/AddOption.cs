using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;
using Votebot.Services;

namespace Votebot.Commands
{
    public class AddOption : ModuleBase<SocketCommandContext>
    {
        public VoteControllerManager VoteControllerManager { get; set; }

        [Command("add"), Summary("Add option(s) to the currently active Options.")]
        public async Task Add(string text)
        {
            Context.Message.DeleteAsync();
            VoteController vc = VoteControllerManager.GetVoteController(Context.Channel);
            foreach (string option in Utils.SeparateOptions(text))
            {
                vc.Options.Add(option);
            }
        }
    }
}
