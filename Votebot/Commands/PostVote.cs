using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Votebot.Controllers;
using Votebot.Models;

namespace Votebot.Commands
{
    public class PostVote : ModuleBase<SocketCommandContext>
    {
        public VoteController VoteController { get; set; }

        [Command("vote", RunMode = RunMode.Async), Alias("v"), Summary("Initiate a vote.")]
        public async Task Vote()
        {
            Context.Message.DeleteAsync();

            if (VoteController.GetOptions().Count == 0)
            {
                VoteController.ResetOptions();
            }

            Vote vote = VoteController.NewVote();
            foreach (string option in VoteController.GetOptions())
            {
                RestUserMessage message = Context.Channel.SendMessageAsync(option).Result;
                vote.AddOptionMessage(message);
                message.AddReactionAsync(new Emoji("👌"));
            }

            await Task.Delay(1000 * ResourceController.GetVoteDelay());
            if (!VoteController.CurrentVote.IsClosed) await VoteController.CloseVote(Context);
        }
    }
}
