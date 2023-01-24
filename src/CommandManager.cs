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
    /// <summary>
    /// A command utility that automatically handles command events and redirects them to implementations of <see cref="CringeCommand"/>.
    /// </summary>
    public static class CommandManager
    {
        private static bool loaded = false;
        private static Dictionary<string, CringeCommand> table;

        /// <summary>Loads the internal command table and registers the commands to the client.</summary>
        /// <param name="client">The client to register commands to.</param>
        public static async Task Load(DiscordSocketClient client)
        {
            if (loaded) // Isn't required, can be loaded per-client and table can be
                return; // static. However, I don't feel like fixing that right now.

            table = new Dictionary<string, CringeCommand>();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(c => c.IsSubclassOf(typeof(CringeCommand))).ToArray();
            ApplicationCommandProperties[] acmds = new ApplicationCommandProperties[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                CringeCommand cmd = (CringeCommand)Activator.CreateInstance(types[i]);
                if (cmd != null)
                {
                    SlashCommandBuilder builder = new SlashCommandBuilder()
                        .WithName(cmd.Name)
                        .WithDescription(cmd.Description);

                    acmds[i] = builder.Build();

                    table.Add(cmd.Name, cmd);
                }
            }

            await client.BulkOverwriteGlobalApplicationCommandsAsync(acmds);

            loaded = true;
        }

        /// <summary>Clears the internal command table.</summary>
        public static Task Unload()
        {
            if (!loaded)
                return Task.CompletedTask;

            table.Clear();
            table = null;

            loaded = false;
            return Task.CompletedTask;
        }

        /// <summary>Runs a Discord command through the internal command table to execute it.</summary>
        /// <param name="command">The command to run.</param>
        public static async Task Run(SocketSlashCommand command)
        {
            if (table.TryGetValue(command.Data.Name, out CringeCommand cmd))
                await cmd.Run(_Main.Bot, command);
            else
                await command.RespondAsync($"Something went wrong, that command could not be found. ({command.Data.Name})");
        }
    }
}
