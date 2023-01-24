using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace Crusader.Commands
{
    public class TodCommand : CringeCommand
    {
        public override string Name => "tod";
        public override string Description => "Gives a new truth or dare prompt.";

        public override async Task Run(SocketSlashCommand command)
        {
            await command.DeferAsync();

            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle("Truth or Dare")
                .WithColor(Color.Purple);

            await command.ModifyOriginalResponseAsync(p =>
            {
                p.Embed = builder.Build();
            });
        }
    }
}
