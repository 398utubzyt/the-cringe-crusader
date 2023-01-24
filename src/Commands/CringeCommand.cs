using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Commands.Builders;

namespace Crusader.Commands
{
    /// <summary>A base class for any slash command implementation found in this repository.</summary>
    public abstract class CringeCommand
    {
        /// <summary>The command name. Must be lower-case with no whitespace.</summary>
        public abstract string Name { get; }
        /// <summary>The command description.</summary>
        public abstract string Description { get; }
        /// <summary>The required permissions to run this command.</summary>
        public virtual GuildPermission Permission { get; } = 0;
        /// <summary>Gets if the command is NSFW.</summary>
        public virtual bool Nsfw { get; } = false;

        /// <summary>Adds custom build steps to the command builder. This is not required.</summary>
        /// <param name="builder">The command builder.</param>
        public virtual Task Build(SlashCommandBuilder builder) { return Task.CompletedTask; }

        /// <summary>Runs the command.</summary>
        /// <param name="bot">TCCC bot instance.</param>
        /// <param name="command">The Discord command context.</param>
        public abstract Task Run(Bot bot, SocketSlashCommand command);

        /// <summary>
        /// Handles component interaction.
        /// This is not required, but is recommended for any command with component interaction.
        /// </summary>
        /// <param name="bot">TCCC bot instance.</param>
        /// <param name="component">The component that was interacted with.</param>
        public virtual Task Handle(Bot bot, SocketMessageComponent component, string id) { return Task.CompletedTask; }
    }
}
