using Discord;
using Discord.WebSocket;

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Crusader.Tod;

namespace Crusader.Commands
{
    /// <summary>/truth</summary>
    public class TruthCommand : TodCommand
    {
        public override string Name => "truth";
        public override string Description => "Gives a new truth prompt.";

        public override Task Run(Bot bot, SocketSlashCommand command) => Impl(bot, command, TodType.Truth, false);
    }
}
