using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Crusader
{
#pragma warning disable IDE1006 // Naming Styles
    internal class _Main
#pragma warning restore IDE1006 // Naming Styles
    {
        private static Task Main(string[] args) => new _Main().AsyncMain();

        private async Task AsyncMain()
        {
            Bot bot = new Bot(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "token.txt")));

            await bot.Start();

            while (bot.Running)
            {

            }

            await bot.Stop();
        }

        private _Main() { } // Prevent instantiation from other classes
    }
}