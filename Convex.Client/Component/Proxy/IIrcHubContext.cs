﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Convex.Client.Component;
using Convex.IRC.Net;

namespace Convex.Client.Models.Proxy {
    public interface IIrcHubContext {
        Task BroadcastMessage(IMessage message);
        Task BroadcastMessageBatch(string connectionId, IEnumerable<IMessage> messageBatch, bool isPrepended);
        Task AddChannel(Channel channel);
        Task RemoveChannel(Channel channel);
        Task UpdateMessageInput(string connectionId, string updatedInput);
    }
}