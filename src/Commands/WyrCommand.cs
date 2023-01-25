using Discord;
using Discord.WebSocket;

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Crusader.Tod;

namespace Crusader.Commands
{
    /// <summary>/wyr</summary>
    public class WyrCommand : CringeCommand
    {
        public override string Name => "wyr";
        public override string Description => "Gives a new Would You Rather prompt.";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected async Task Impl(Bot bot, IDiscordInteraction interaction, bool button)
        {
            await interaction.DeferAsync();

            TodPrompt prompt = bot.TruthOrDare.GetWyr();

            EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor($"Requested by {interaction.User.Username}#{interaction.User.Discriminator}", interaction.User.GetAvatarUrl())
                .WithTitle(prompt.Text)
                .WithColor(Color.Blue)
                .WithFooter($"Type: WYR");

            if (!button)
            {
                await interaction.ModifyOriginalResponseAsync(p =>
                {
                    p.Embed = builder.Build();
                    p.Components = new ComponentBuilder().AddRow(new ActionRowBuilder()
                        .WithButton("Would You Rather", "wyr__wyr", ButtonStyle.Primary)).Build();
                });
            } else
            {
                await interaction.ModifyOriginalResponseAsync(p => { p.Components = new ComponentBuilder().Build(); });
                await interaction.FollowupAsync(embed: builder.Build(), 
                    components: new ComponentBuilder().AddRow(new ActionRowBuilder()
                        .WithButton("Would You Rather", "wyr__wyr", ButtonStyle.Primary)).Build());
            }
            
        }

        public override Task Run(Bot bot, SocketSlashCommand command) => Impl(bot, command, false);
        public override Task Handle(Bot bot, SocketMessageComponent component, string id)
            => Impl(bot, component, true);
    }
}
