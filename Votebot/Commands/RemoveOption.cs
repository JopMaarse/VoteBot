using System.Linq;
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
        public async Task Remove(string text)
        {
            Context.Message.DeleteAsync();
            string[] options = Utils.SeparateOptions(text).ToArray();
            await Context.Channel.SendMessageAsync(
                VoteControllerManager.GetVoteController(Context.Channel)
                    .RemoveOptions(options).All(s => s)
                    ? $"All input removed form options."
                    : $"One or more input values were not an option.");
        }
    }
}
