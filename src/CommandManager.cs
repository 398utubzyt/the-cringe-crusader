using Crusader.Commands;

using Discord;
using Discord.Commands.Builders;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace Crusader
{
    /// <summary>
    /// A command utility that automatically handles command events and redirects them to implementations of <see cref="CringeCommand"/>.
    /// </summary>
    public static class CommandManager
    {
        private static bool loaded = false;
        private static Dictionary<string, CringeCommand> table;

        /// <summary>Gets the enumerator of the internal command table.</summary>
        /// <returns>An <see cref="IEnumerator{T}"/> which can be used to iterate through the command table.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<CringeCommand> EnumerateCommands() => table.Values.GetEnumerator();
        /// <summary>Gets the amount of commands in the internal command table.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CommandCount() => table.Count;

        private class CommandEqualityComparer : IEqualityComparer<ApplicationCommandProperties>
        {
            public bool Equals(ApplicationCommandProperties x, ApplicationCommandProperties y)
            {
                return x.Name.GetValueOrDefault() == y.Name.GetValueOrDefault();
            }

            public int GetHashCode([DisallowNull] ApplicationCommandProperties obj)
            {
                return obj.Name.GetHashCode();
            }
        }

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
                        .WithDescription(cmd.Description)
                        .WithDefaultPermission(true)
                        .WithDefaultMemberPermissions(GuildPermission.UseApplicationCommands | cmd.Permission)
                        .WithNsfw(cmd.Nsfw)
                        .WithDMPermission(false);
                    await cmd.Build(builder);

                    acmds[i] = builder.Build();
                    table.Add(cmd.Name, cmd);
                }
            }

            //await client.BulkOverwriteGlobalApplicationCommandsAsync(acmds);
            foreach (SocketGuild guild in client.Guilds)
            {
                await guild.BulkOverwriteApplicationCommandAsync(acmds);

                IReadOnlyCollection<SocketApplicationCommand> rocmds = await guild.GetApplicationCommandsAsync();
                foreach (ApplicationCommandProperties prop in acmds)
                {
                    bool contains = false;
                    foreach (SocketApplicationCommand sac in rocmds)
                        if (sac.Name == prop.Name.GetValueOrDefault())
                            contains = true;
                    if (!contains)
                        await guild.CreateApplicationCommandAsync(prop).ConfigureAwait(false);
                }
            }

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
                await command.RespondAsync($"Something went wrong, that command could not be found. ({command.Data.Name})", null, false, true);
        }

        /// <summary>Handles a Discord component interaction.</summary>
        /// <param name="component">The component that was interacted with.</param>
        public static async Task Handle(SocketMessageComponent component)
        {
            int index = component.Data.CustomId.IndexOf("__");
            string name = component.Data.CustomId.Remove(index);
            if (table.TryGetValue(name, out CringeCommand cmd))
                await cmd.Handle(_Main.Bot, component, component.Data.CustomId[(index + 2)..]);
            else
                await component.RespondAsync($"Something went wrong, that command could not be found. ({name})", null, false, true);
        }
    }
}
