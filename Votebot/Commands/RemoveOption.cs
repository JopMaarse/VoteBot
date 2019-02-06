using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;

namespace Votebot.Commands
{
    public class RemoveOption : ModuleBase<SocketCommandContext>
    {
        public VoteController VoteController { get; set; }

        [Command("remove"), Summary("Remove one option from the currently active options.")]
        public async Task Reset(string option)
        {
            Context.Message.DeleteAsync();
            string message = VoteController.RemoveOption(option)
                ? $"Successfully removed {option}."
                : $"{option} is not an option.";
            await Context.Channel.SendMessageAsync(message);
        }
    }
}
