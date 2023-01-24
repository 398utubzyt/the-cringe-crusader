using Discord;
using Discord.WebSocket;

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Crusader.Tod;

namespace Crusader.Commands
{
    /// <summary>/tod</summary>
    public class TodCommand : CringeCommand
    {
        public override string Name => "tod";
        public override string Description => "Gives a new truth or dare prompt.";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetTypeName(TodType type)
            => type switch { TodType.Truth => "Truth", TodType.Dare => "Dare", _ => "Unknown" };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetTypeColor(TodType type)
            => type switch { TodType.Truth => Color.Green, TodType.Dare => Color.Red, _ => Color.LightGrey };

        public override async Task Run(Bot bot, SocketSlashCommand command)
        {
            await command.DeferAsync();

            TodPrompt prompt = bot.TruthOrDare.GetRandom();

            EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor($"Requested by {command.User.Username}#{command.User.Discriminator}")
                .WithTitle(prompt.Text)
                .WithColor(GetTypeColor(prompt.Type))
                .WithFooter($"Type: {GetTypeName(prompt.Type)}");

            await command.ModifyOriginalResponseAsync(p =>
            {
                p.Embed = builder.Build();
            });
        }
    }
}
