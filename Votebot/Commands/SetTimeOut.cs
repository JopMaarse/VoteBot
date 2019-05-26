using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;

namespace Votebot.Commands
{
    public class SetTimeOut : ModuleBase<SocketCommandContext>
    {
        [Command("timeout"), Summary("Set the time until votes are automatically closed in seconds.")]
        public async Task Reset(int time)
        {
            Context.Message.DeleteAsync();
            ResourceController.SetVoteDelay(time);
            await Context.Channel.SendMessageAsync($"Timeout is now {time} seconds.");
        }

        [Command("timeout"), Summary("Set the time until votes are automatically closed in seconds.")]
        public async Task Reset(string NaN)
        {
            Context.Message.DeleteAsync();
            await Context.Channel.SendMessageAsync($"{NaN} is not a number. Example: !timeout 120");
        }
    }
}
