using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;
using Votebot.Services;

namespace Votebot.Commands
{
    public class SetOptions : ModuleBase<SocketCommandContext>
    {
        public VoteControllerManager VoteControllerManager { get; set; }

        [Command("Options"), Alias("o"), Summary("Set the vote Options. Separate each option with \'|\' without the quotation marks.")]
        public async Task Options([Remainder] string text)
        {
            Context.Message.DeleteAsync();
            string[] options = text.Split('|')
                .Select(o => o.TrimStart().TrimEnd())
                .ToArray();
            await Task.Run(() => VoteControllerManager.GetVoteController(Context.Channel).SetOptions(options));
        }
    }
}
