using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Commands.Builders;

namespace Crusader.Commands
{
    public abstract class CringeCommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public virtual GuildPermission Permission { get; } = 0;
        public virtual bool Nsfw { get; } = false;
        public virtual Task Build(SlashCommandBuilder builder) { return Task.CompletedTask; }
        public abstract Task Run(SocketSlashCommand command);
    }
}
