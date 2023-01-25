using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;
using System.Linq;

namespace Crusader.Commands
{
    /// <summary>/dump</summary>
    public class DumpCommand : CringeCommand
    {
        public override string Name => "dump";
        public override string Description => "Dumps the bot state onto persistent storage.";
        public override GuildPermission Permission => GuildPermission.ManageGuild;

        public override async Task Run(Bot bot, SocketSlashCommand command)
        {
            await command.DeferAsync(true);

            try
            {
                await bot.Dump();

                await command.ModifyOriginalResponseAsync(p => { p.Content = "Current state has been dumped!"; });
            } catch (System.Exception e)
            {
                await command.ModifyOriginalResponseAsync(p => {
                    p.Content = $"Exception '{e.GetType().FullName}' thrown while dumping: \"{e.Message}\"\nPlease report this error.";
                });
            }
        }
    }
}
