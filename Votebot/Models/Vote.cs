using System;
using System.Collections.Generic;
using System.Threading;
using Discord.Rest;

namespace Votebot.Models
{
    public class Vote
    {
        public ICollection<RestUserMessage> Options { get; }
        public DateTime TimeStamp { get; }
        public bool IsClosed { get; set; }
        
        public Vote()
        {
            IsClosed = false;
            TimeStamp = DateTime.Now;
            Options = new List<RestUserMessage>();
        }

        public void AddOptionMessage(RestUserMessage option)
        {
            Options.Add(option);
        }
    }
}
