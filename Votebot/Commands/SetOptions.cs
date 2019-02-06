using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using Votebot.Controllers;

namespace Votebot.Commands
{
    public class SetOptions : ModuleBase<SocketCommandContext>
    {
        public VoteController VoteController { get; set; }

        [Command("options"), Alias("o"), Summary("Set the vote options. Separate each option with \'|\' without the quotation marks.")]
        public async Task Options([Remainder] string text)
        {
            Context.Message.DeleteAsync();
            string[] options = text.Split('|')
                .Select(o => o.TrimStart().TrimEnd())
                .ToArray();
            await Task.Run(() => VoteController.SetOptions(options));
        }
    }
}
