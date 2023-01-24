using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Crusader.Tod;
using Crusader.Confess;
using System.Net.NetworkInformation;

namespace Crusader
{
    public class Bot : IDumpable
    {
        private readonly string token;
        private readonly DiscordSocketClient client;
        //private readonly CommandService commands;

        public DiscordSocketClient Client => client;

        private TruthOrDare tod;
        private Confessions confess;
        public TruthOrDare TruthOrDare => tod;
        public Confessions Confessions => confess;

        private bool running;
        private bool offline;
        public bool Running => running;
        public bool Offline => offline;

        #region Start/Stop

        public async Task Start()
        {
            running = true;

            try
            {
                using Ping p = new Ping();
                PingReply reply = await p.SendPingAsync("google.com", 1000);
                if (reply.Status != IPStatus.Success)
                    offline = true;
            } catch (System.Exception)
            {
                offline = true;
            }

            if (!offline)
            {
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
            } else
                await Logger.Warn("Entering offline mode...");
        }

        public async Task Stop()
        {
            running = false;
            if (offline)
            {
                await client.StopAsync();
                await client.LogoutAsync();
            }
        }

        #endregion

        #region Events

        public async Task Ready()
        {
            await CommandManager.Load(client);
        }

        #endregion

        #region Dump

        public async Task Dump()
        {
            await tod.Dump();
            await confess.Dump();
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

            tod = new TruthOrDare();
            confess = new Confessions();
        }
    }
}
