using Discord;
using Discord.WebSocket;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace Crusader.Commands
{
    /// <summary>/ping</summary>
    public class PingCommand : CringeCommand
    {
        public override string Name => "ping";
        public override string Description => "Pings the bot.";

        public override async Task Run(Bot bot, SocketSlashCommand command)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            await command.DeferAsync(true);
            watch.Stop();
            
            await command.ModifyOriginalResponseAsync(p => 
            { p.Content = $"Pong! Latency is {watch.Elapsed.Milliseconds}ms ({watch.Elapsed.TotalSeconds}s)"; });
        }
    }
}
