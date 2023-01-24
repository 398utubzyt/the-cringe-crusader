using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Crusader
{
    public static class _Main
    {
        private static Bot bot;
        public static Bot Bot => bot;
        public static Task Main() => AsyncMain();

        private static async Task AsyncMain()
        {
            bot = new Bot(File.ReadAllText(FileUtil.Root("token.txt")));
            
            await bot.Start();

            while (bot.Running)
            {
                string[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (input.Length > 0)
                {
                    switch (input[0].ToLower())
                    {
                        case "stop":
                            await bot.Stop();
                            await bot.Dump();
                            break;

                        case "dump":
                            await Logger.Info("Dumping bot data...");
                            await bot.Dump();
                            await Logger.Info("Dump finished.");
                            break;

                        case "tod":
                            await Logger.Info(Bot.TruthOrDare.GetRandom().Text);
                            break;

                        case "confess":
                            Bot.Confessions.Create(0, 0, string.Join(' ', input[1..]));
                            await Logger.Info("Confession logged.");
                            break;
                    }
                }
            }

            await CommandManager.Unload();
        }

        //private _Main() { } // Prevent instantiation from other classes
    }
}