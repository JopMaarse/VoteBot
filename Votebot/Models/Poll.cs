using System;
using System.Collections.Generic;
using System.Threading;
using Discord.Rest;
using Votebot.Controllers;

namespace Votebot.Models
{
    public class Poll
    {
        public ICollection<RestUserMessage> Options { get; }
        public DateTime TimeStamp { get; }
        public bool IsClosed { get; set; }
        public bool MultipleVotesAllowed { get; set; }
        
        public Poll()
        {
            IsClosed = false;
            TimeStamp = DateTime.Now;
            Options = new List<RestUserMessage>();
            MultipleVotesAllowed = ResourceController.GetMultipleOptionsAllowed();
        }

        public void AddOptionMessage(RestUserMessage option)
        {
            Options.Add(option);
        }
    }
}
