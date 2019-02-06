using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Rest;
using MoreLinq;
using Votebot.Models;

namespace Votebot.Controllers
{
    public class VoteController
    {
        protected IList<string> options;

        public Vote CurrentVote { get; protected set; }

        public VoteController()
        {
            ResetOptions();
        }

        public void ResetOptions()
        {
            options = ResourceController.GetDefaultOptions();
        }

        public void SetOptions(params string[] options)
        {
            this.options = options.ToList();
        }

        public void SetDefaultOptions(params string[] options)
        {
            ResourceController.SetDefaultOptions(options);
        }

        public IList<string> GetOptions()
        {
            return options;
        }

        public bool RemoveOption(string option)
        {
            return options.Remove(option);
        }

        public async Task CloseVote(SocketCommandContext context)
        {
            CurrentVote.IsClosed = true;

            foreach (RestUserMessage message in CurrentVote.Options)
            {
                await message.UpdateAsync();
            }

            RestUserMessage[] winnerMessage = CurrentVote.Options.
                MaxBy(o => o.Reactions.Sum(r => r.Value.ReactionCount))
                .ToArray();

            if (winnerMessage.Length != 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("There is a tie between");
                foreach (RestUserMessage message in winnerMessage)
                {
                    sb.Append(" ");
                    sb.Append(message.Content);
                }

                await context.Channel.SendMessageAsync(sb.ToString());
            }
            else
            {
                string winner = winnerMessage[0].Content;
                options.Remove(winner);

                foreach (RestUserMessage message in CurrentVote.Options)
                {
                    message.DeleteAsync();
                }

                await context.Channel.SendMessageAsync($"The winner is {winner}");
            }
        }

        public Vote NewVote()
        {
            CurrentVote = new Vote();
            return CurrentVote;
        }
    }
}
