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

        public bool Running => client.ConnectionState == ConnectionState.Connected;

        #region Start/Stop

        public async Task Start()
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
        }

        public async Task Stop()
        {
            await client.StopAsync();
            await client.LogoutAsync();
        }

        #endregion

        #region Events

        public async Task Ready()
        {
            await Logger.Info("Ready");
            
            
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
