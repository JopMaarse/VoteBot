using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;
using Votebot.Services;

namespace Votebot.Commands
{
    public class ResetOptions : ModuleBase<SocketCommandContext>
    {
        public VoteControllerManager VoteControllerManager { get; set; }

        [Command("reset"), Alias("r"), Summary("Reset the vote Options to the default Options.")]
        public async Task Reset()
        {
            Context.Message.DeleteAsync();
            await Task.Run(() => VoteControllerManager.GetVoteController(Context.Channel).ResetOptions());
        }
    }
}
