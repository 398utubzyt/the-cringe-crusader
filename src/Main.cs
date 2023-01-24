using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Crusader
{
    public static class _Main
    {
        public static Task Main() => AsyncMain();

        private static async Task AsyncMain()
        {
            Bot bot = new Bot(File.ReadAllText(FileUtil.Root("token.txt")));

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
                            break;

                        case "dump":
                            await bot.DumpState();
                            break;
                    }
                }
            }

            await CommandManager.Unload();
        }

        //private _Main() { } // Prevent instantiation from other classes
    }
}