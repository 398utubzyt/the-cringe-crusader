using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Crusader
{
    /// <summary>The class of the main entry point.</summary>
    public static class _Main
    {
        private static Bot bot;
        /// <summary>I tried, but this API forced me to use a static reference. :(</summary>
        public static Bot Bot => bot;
        /// <summary>Main entry point.</summary>
        public static Task Main() => AsyncMain();

        /// <summary>Asynchronous entry point.</summary>
        private static async Task AsyncMain()
        {
            if (!FileUtil.Ensure(FileUtil.Root("token.txt")))
            {
                await Logger.Error("Please ensure that a bot token exists.");
                return;
            }
            // Create bot with new token.
            bot = new Bot(File.ReadAllText(FileUtil.Root("token.txt")));
            
            await bot.Start();

            // Run dev console while bot is running.
            // Just in case I fucked up somewhere.
            while (bot.Running)
            {
                string[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (input.Length > 0)
                {
                    switch (input[0].ToLower())
                    {
                        // Offline: Client-side command line.
                        // This doesn't have to look neat,
                        // since no one is using it.
                        //
                        // E.g. no need for subclasses and
                        // assembly reflection
                        case "stop":
                            await bot.Stop();
                            await bot.Dump();
                            break;

                        // Dump bot data onto drive.
                        case "dump":
                            await Logger.Info("Dumping bot data...");
                            await bot.Dump();
                            await Logger.Info("Dump finished.");
                            break;

                        // Generate a Truth or Dare prompt.
                        case "tod":
                            await Logger.Info(Bot.TruthOrDare.GetTruthOrDare().Text);
                            break;

                        // Logs a confession.
                        case "confess":
                            if (input.Length < 2)
                                break;
                            Confess.Confession c = bot.Confessions.Create(Discord.SnowflakeUtils.ToSnowflake(DateTimeOffset.Now), 
                                bot.Offline ? 0 : bot.Client.CurrentUser.Id, string.Join(' ', input[1..]));
                            await Logger.Info($"Confession logged ({c.Id}).");
                            break;

                        // Logs a confession.
                        case "cquery":
                            if (input.Length < 2)
                                break;
                            Guid id;
                            if (!Guid.TryParse(input[1], out id))
                                break;
                            Confess.Confession c2 = bot.Confessions.Get(id);
                            if (!c2.Id.Equals(Guid.Empty))
                            {
                                await Logger.Info($"Confession {c2.Id}");
                                await Logger.Info($"At {Discord.SnowflakeUtils.FromSnowflake(c2.Date).LocalDateTime}");
                                Discord.IUser user = c2.Author != 0 ? await bot.Client.GetUserAsync(c2.Author) : null;
                                await Logger.Info($"From '{(user != null ? $"{user.Username}#{user.Discriminator}" : "User not found")}' ({c2.Author})");
                            }
                            await Logger.Info($"\"{c2.Message}\"");
                            break;
                    }
                }
            }

            // Clear the decently-sized dictionary that was created at startup.
            await CommandManager.Unload();
        }

        //private _Main() { } // Prevent instantiation from other classes
    }
}