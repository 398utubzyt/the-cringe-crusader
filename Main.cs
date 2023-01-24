using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Crusader
{
    internal class _Main
    {
        private static Task Main(string[] args) => new _Main().AsyncMain();

        private async Task AsyncMain()
        {
            
            Bot bot = new Bot(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "token.txt")));
        }

        private _Main() { } // Prevent instantiation from other classes
    }
}