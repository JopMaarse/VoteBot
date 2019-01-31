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
            string[] options = text.Split('|');

            for (int i = 0; i < options.Length; i++)
            {
                options[i] = options[i].TrimStart();
                options[i] = options[i].TrimEnd();
            }

            await Task.Run(() => VoteController.SetOptions(options));
        }
    }
}
