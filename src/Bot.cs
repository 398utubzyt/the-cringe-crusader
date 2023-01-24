using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Crusader.Tod;
using Crusader.Confess;
using System.Net.NetworkInformation;

namespace Crusader
{
    /// <summary>
    /// A bot class from handling all TCCC bot logic.
    /// </summary>
    public class Bot : IDumpable
    {
        private readonly string token;
        private readonly DiscordSocketClient client;
        //private readonly CommandService commands;

        /// <summary>The Discord client.</summary>
        public DiscordSocketClient Client => client;

        private TruthOrDare tod;
        private Confessions confess;
        /// <summary>A Truth or Dare instance.</summary>
        public TruthOrDare TruthOrDare => tod;
        /// <summary>A Confessions instance.</summary>
        public Confessions Confessions => confess;

        private bool running;
        private bool offline;
        /// <summary>Gets whether the bot is running right now or not.</summary>
        public bool Running => running;
        /// <summary>Gets whether the bot is in offline mode or not.</summary>
        public bool Offline => offline;

        /// <summary>Starts the bot.</summary>
        public async Task Start()
        {
            running = true;

            try
            {
                using Ping p = new Ping();
                PingReply reply = await p.SendPingAsync("discord.com", 1000);
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

        /// <summary>Stops the bot.</summary>
        public async Task Stop()
        {
            running = false;
            if (offline)
            {
                await client.StopAsync();
                await client.LogoutAsync();
            }
        }

        public async Task Dump()
        {
            await tod.Dump();
            await confess.Dump();
        }

        // This section implements any required Discord event for the bot to function.
        #region Events

        private async Task Ready()
        {
            await CommandManager.Load(client);
        }

        #endregion

        /// <summary>Creates a new bot with a <paramref name="token"/>.</summary>
        /// <param name="token">The bot token. It is very important that this is never shared.</param>
        public Bot(string token)
        {
            client = new DiscordSocketClient(new DiscordSocketConfig { GatewayIntents = GatewayIntents.All & ~(
                GatewayIntents.GuildPresences |
                GatewayIntents.GuildInvites |
                GatewayIntents.GuildScheduledEvents |
                GatewayIntents.DirectMessages |
                GatewayIntents.DirectMessageTyping |
                GatewayIntents.DirectMessageReactions
            )});
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
