using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Crusader
{
    public class Bot
    {
        private readonly string token;
        private readonly DiscordSocketClient client;
        //private readonly CommandService commands;

        private bool running;
        public bool Running => running;

        #region Start/Stop

        public async Task Start()
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            running = true;
        }

        public async Task Stop()
        {
            running = false;
            await client.StopAsync();
            await client.LogoutAsync();
        }

        #endregion

        #region Events

        public async Task Ready()
        {
            await CommandManager.Load(client);
        }

        #endregion

        #region Dump

        public async Task DumpState()
        {
            
        }

        #endregion

        public Bot(string token)
        {
            client = new DiscordSocketClient();
            //commands = new CommandService(new CommandServiceConfig() { ThrowOnError = false });

            this.token = token;

            client.Log += Logger.Log;
            client.SlashCommandExecuted += CommandManager.Run;

            client.Ready += Ready;
        }
    }
}
