using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Votebot.Controllers;
using Votebot.Models;
using Votebot.Services;

namespace Votebot.Commands
{
    public class PostVote : ModuleBase<SocketCommandContext>
    {
        public VoteControllerManager VoteControllerManager { get; set; }

        [Command("vote", RunMode = RunMode.Async), Alias("v"), Summary("Initiate a vote.")]
        public async Task Vote()
        {
            Context.Message.DeleteAsync();

            VoteController vc = VoteControllerManager.GetVoteController(Context.Channel);

            if (vc.GetOptions().Count == 0)
            {
                vc.ResetOptions();
            }

            Poll poll = vc.NewPoll();
            foreach (string option in vc.GetOptions())
            {
                RestUserMessage message = Context.Channel.SendMessageAsync(option).Result;
                poll.AddOptionMessage(message);
                message.AddReactionAsync(new Emoji("👌"));
            }

            await Task.Delay(1000 * ResourceController.GetVoteDelay());
            if (!vc.CurrentPoll.IsClosed) await vc.ClosePoll(Context);
        }

        [Command("vote", RunMode = RunMode.Async), Alias("v"), Summary("Initiate a vote with custom options.")]
        public async Task Vote([Remainder] string text)
        {
            Context.Message.DeleteAsync();

            VoteController vc = VoteControllerManager.GetVoteController(Context.Channel);
            vc.SetOptions(text);

            if (vc.GetOptions().Count == 0)
            {
                vc.ResetOptions();
            }

            Poll vote = vc.NewPoll();
            foreach (string option in vc.GetOptions())
            {
                RestUserMessage message = Context.Channel.SendMessageAsync(option).Result;
                vote.AddOptionMessage(message);
                message.AddReactionAsync(new Emoji("👌"));
            }

            await Task.Delay(1000 * ResourceController.GetVoteDelay());
            if (!vc.CurrentPoll.IsClosed) await vc.ClosePoll(Context);
        }
    }
}
