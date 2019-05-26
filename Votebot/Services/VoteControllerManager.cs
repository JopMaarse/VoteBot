using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Votebot.Controllers;

namespace Votebot.Services
{
    public class VoteControllerManager
    {
        private Dictionary<ulong, VoteController> voteControllers;
        private readonly DiscordSocketClient _client;

        public VoteControllerManager(IServiceProvider serviceProvider, DiscordSocketClient client)
        {
            voteControllers = new Dictionary<ulong, VoteController>();
            _client = client;
        }

        public VoteController GetVoteController(ISocketMessageChannel channel)
        {
            if (!voteControllers.ContainsKey(channel.Id))
            {
                voteControllers.Add(channel.Id, new VoteController(_client));
            }

            return voteControllers[channel.Id];
        }
    }
}
