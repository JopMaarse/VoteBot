using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;

namespace Votebot.Commands
{
    public class CloseVote : ModuleBase<SocketCommandContext>
    {
        public VoteController VoteController { get; set; }

        [Command("close"), Alias("c"), Summary("Close the current vote early.")]
        public async Task Close()
        {
            Context.Message.DeleteAsync();
            await VoteController.CloseVote(Context);
        }
    }
}
