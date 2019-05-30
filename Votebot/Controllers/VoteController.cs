using Discord.Commands;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using MoreLinq;
using Votebot.Models;

namespace Votebot.Controllers
{
    public class VoteController
    {
        private readonly DiscordSocketClient _client;

        public ICollection<string> Options { get; set; }
        public Dictionary<ulong, IList<SocketReaction>> UserVotes { get; set; }
        public Poll CurrentPoll { get; protected set; }
        public bool MultipleVotesAllowed { get; set; }

        public VoteController(DiscordSocketClient client)
        {
            ResetOptions();
            UserVotes = new Dictionary<ulong, IList<SocketReaction>>();
            _client = client;
            MultipleVotesAllowed = ResourceController.GetMultipleOptionsAllowed();
        }

        public void ResetOptions()
        {
            Options = ResourceController.GetDefaultOptions();
        }

        public void SetOptions(string text)
        {
            Options = Utils.SeparateOptions(text).ToList();
        }

        public void SetDefaultOptions(params string[] options)
        {
            ResourceController.SetDefaultOptions(options);
        }

        public ICollection<string> GetOptions()
        {
            return Options;
        }

        public bool[] RemoveOptions(params string[] options)
        {
            bool[] result = new bool[options.Length];
            for(int i = 0; i < options.Length; i++)
            {
                result[i] = Options.Remove(options[i]);
            }

            return result;
        }

        public async Task ClosePoll(SocketCommandContext context)
        {
            CurrentPoll.IsClosed = true;

            string[] winnerMessages = CalculateWinners().ToArray();

            if (winnerMessages.Length > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("There is a tie between");
                foreach (string message in winnerMessages)
                {
                    sb.Append(" ");
                    sb.Append(message);
                }

                await context.Channel.SendMessageAsync(sb.ToString());
            }
            else
            {
                string winner = winnerMessages.Single();
                Options.Remove(winner);
                UserVotes = new Dictionary<ulong, IList<SocketReaction>>();

                foreach (RestUserMessage message in CurrentPoll.Options)
                {
                    message.DeleteAsync();
                }

                await context.Channel.SendMessageAsync($"The winner is {winner}");
            }
        }

        public Poll NewPoll()
        {
            CurrentPoll = new Poll();
            return CurrentPoll;
        }

        protected IEnumerable<string> CalculateWinners()
        {
            return UserVotes.Select(votes => votes.Value.Select(reaction => reaction.Message.Value.Content).Distinct())
                            .SelectMany(votes => votes)
                            .GroupBy(vote => vote)
                            .MaxBy(grouping => grouping.Count())
                            .Select(grouping => grouping.Key);
        }

        public void AddVote(SocketReaction reaction)
        {
            if (MultipleVotesAllowed)
            {
                if (!UserVotes.ContainsKey(reaction.UserId))
                {
                    UserVotes.Add(reaction.UserId, new List<SocketReaction>());
                }
                UserVotes[reaction.UserId].Add(reaction);
            }
            else
            {
                if (UserVotes.ContainsKey(reaction.UserId) && _client.CurrentUser.Id != reaction.UserId)
                {
                    SocketReaction oldReaction = UserVotes[reaction.UserId].Single();
                    RemoveVote(oldReaction);
                }
                else
                {
                    UserVotes.Add(reaction.UserId, new List<SocketReaction>());
                }
                UserVotes[reaction.UserId].Add(reaction);
            }
        }

        public void RemoveVote(SocketReaction reaction)
        {
            reaction.Message.Value.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
            UserVotes[reaction.UserId].Remove(reaction);
        }
    }
}
