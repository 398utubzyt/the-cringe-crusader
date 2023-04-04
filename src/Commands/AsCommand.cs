using Discord;
using Discord.WebSocket;

/*
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace Crusader.Commands
{
    /// <summary>/as</summary>
    public class AsCommand : CringeCommand
    {
        public override string Name => "as";
        public override string Description => "Send a message as a custom user.";

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
*/