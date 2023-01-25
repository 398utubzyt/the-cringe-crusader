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
            => type switch { TodType.Truth => "TRUTH", TodType.Dare => "DARE", TodType.Wyr => "WYR", _ => "UNKNOWN" };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetTypeColor(TodType type)
            => type switch { TodType.Truth => Color.Green, TodType.Dare => Color.Red, TodType.Wyr => Color.Blue, _ => Color.LightGrey };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected async Task Impl(Bot bot, IDiscordInteraction interaction, TodType type, bool button)
        {
            await interaction.DeferAsync();

            TodPrompt prompt = type switch {
                TodType.Truth => bot.TruthOrDare.GetTruth(),
                TodType.Dare => bot.TruthOrDare.GetDare(),
                TodType.Wyr => bot.TruthOrDare.GetWyr(),
                _ => bot.TruthOrDare.GetTruthOrDare(),
            };

            EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor($"Requested by {interaction.User.Username}#{interaction.User.Discriminator}", interaction.User.GetAvatarUrl())
                .WithTitle(prompt.Text)
                .WithColor(GetTypeColor(prompt.Type))
                .WithFooter($"Type: {GetTypeName(prompt.Type)}");

            if (!button)
            {
                await interaction.ModifyOriginalResponseAsync(p =>
                {
                    p.Embed = builder.Build();
                    p.Components = CommandUtil.GreenRedBlurpleButtons(this, "truth", "dare", "random", "Truth", "Dare", "Random");
                });
            } else
            {
                await interaction.ModifyOriginalResponseAsync(p => { p.Components = new ComponentBuilder().Build(); });
                await interaction.FollowupAsync(embed: builder.Build(), 
                    components: CommandUtil.GreenRedBlurpleButtons(this, "truth", "dare", "random", "Truth", "Dare", "Random"));
            }
        }

        public override Task Run(Bot bot, SocketSlashCommand command) => Impl(bot, command, 0, false);
        public override Task Handle(Bot bot, SocketMessageComponent component, string id)
            => Impl(bot, component, id switch { "truth" => TodType.Truth, "dare" => TodType.Dare, "wyr" => TodType.Wyr, _ => 0 }, true);
    }
}
