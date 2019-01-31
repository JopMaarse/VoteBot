using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;

namespace Votebot.Commands
{
    public class ResetOptions : ModuleBase<SocketCommandContext>
    {
        public VoteController VoteController { get; set; }

        [Command("reset"), Alias("r"), Summary("Reset the vote options to the default options.")]
        public async Task Reset()
        {
            Context.Message.DeleteAsync();
            await Task.Run(() => VoteController.ResetOptions());
        }
    }
}
