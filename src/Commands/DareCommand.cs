using Discord;
using Discord.WebSocket;

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Crusader.Tod;

namespace Crusader.Commands
{
    /// <summary>/dare</summary>
    public class DareCommand : TodCommand
    {
        public override string Name => "dare";
        public override string Description => "Gives a new dare prompt.";

        public override Task Run(Bot bot, SocketSlashCommand command) => Impl(bot, command, TodType.Dare, false);
    }
}
