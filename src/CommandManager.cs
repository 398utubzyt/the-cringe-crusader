using Crusader.Commands;

using Discord;
using Discord.Commands.Builders;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Crusader
{
    public static class CommandManager
    {
        private static bool loaded = false;
        private static Dictionary<string, CringeCommand> table;
        public static async Task Load(DiscordSocketClient client)
        {
            if (loaded)
                return;

            table = new Dictionary<string, CringeCommand>();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(c => c.IsSubclassOf(typeof(CringeCommand))).ToArray();
            for (int i = 0; i < types.Length; i++)
            {
                CringeCommand cmd = (CringeCommand)Activator.CreateInstance(types[i]);
                if (cmd != null)
                {
                    table.Add(cmd.Name, cmd);
                    SlashCommandBuilder builder = new SlashCommandBuilder()
                        .WithName(cmd.Name)
                        .WithDescription(cmd.Description)
                        .WithDMPermission(false)
                        .WithDefaultPermission(cmd.Permission == 0)
                        .WithDefaultMemberPermissions(cmd.Permission)
                        .WithNsfw(cmd.Nsfw);
                    await cmd.Build(builder);
                    await client.CreateGlobalApplicationCommandAsync(builder.Build());
                }
            }

            loaded = true;
        }

        public static Task Unload()
        {
            if (!loaded)
                return Task.CompletedTask;

            table.Clear();
            table = null;

            loaded = false;
            return Task.CompletedTask;
        }

        public static async Task Run(SocketSlashCommand command)
        {
            if (table.TryGetValue(command.Data.Name, out CringeCommand cmd))
                await cmd.Run(command);
        }
    }
}
