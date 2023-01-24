using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Crusader
{
    public class Bot
    {
        private string token;
        private DiscordSocketClient client;
        private CommandService commands;

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

        public Bot(string token)
        {
            client = new DiscordSocketClient();
            commands = new CommandService(new CommandServiceConfig() { ThrowOnError = false });

            this.token = token;

            client.Log += Logger.Log;
        }
    }
}
