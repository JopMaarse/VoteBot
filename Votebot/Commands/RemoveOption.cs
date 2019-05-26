using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;
using Votebot.Services;

namespace Votebot.Commands
{
    public class RemoveOption : ModuleBase<SocketCommandContext>
    {
        public VoteControllerManager VoteControllerManager { get; set; }

        [Command("remove"), Summary("Remove one option from the currently active Options.")]
        public async Task Reset(string option)
        {
            Context.Message.DeleteAsync();
            string message = VoteControllerManager.GetVoteController(Context.Channel).RemoveOption(option)
                ? $"Successfully removed {option}."
                : $"{option} is not an option.";
            await Context.Channel.SendMessageAsync(message);
        }
    }
}
