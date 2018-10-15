﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Convex.IRC.Component;
using Convex.IRC.Dependency;

namespace Convex.Client.Services {
    public class IrcHostedService : IIrcHostedService
    {
        public IrcHostedService() {
            Client = new IRC.Client();
            Messages = new List<ServerMessage>();

            Address = "irc.foonetic.net";
            Port = 6667;
        }

        #region MEMBERS
        
        public IClient Client { get; }

        public string Address { get; }
        public int Port { get; }

        public List<ServerMessage> Messages { get; }

        #endregion

        #region METHODS

        private async Task DoWork() {
            await Client.BeginListenAsync();
        }

        #endregion

        #region INIT

        private async Task Initialise() {
            await InitialiseClient();
        }

        private async Task InitialiseClient() {
            Client.Logged += (sender, args) => {
                Debug.WriteLine(args.Information);
                return Task.CompletedTask;
            };
            await Client.Initialise(Address, Port);
        }

        #endregion

        #region INTERFACE IMPLEMENTATION

        public async Task StartAsync(CancellationToken cancellationToken) {
            await Initialise();
            await DoWork();
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            Dispose();

            return Task.CompletedTask;
        }

        public void Dispose() {
            Client.Dispose();
        }

        #endregion
    }
}