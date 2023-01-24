using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;
using System.Linq;

namespace Crusader.Commands
{
    public class WhoCommand : CringeCommand
    {
        public override string Name => "who";
        public override string Description => "Look up a user with their user ID.";
        public override GuildPermission Permission => GuildPermission.ManageGuild;

        public override Task Build(SlashCommandBuilder builder)
        {
            builder.AddOption(
                new SlashCommandOptionBuilder()
                .WithName("id")
                .WithDescription("The ID of the user to look for.")
                .WithType(ApplicationCommandOptionType.String)
                .WithRequired(true)
            );
            return Task.CompletedTask;
        }

        public override async Task Run(Bot bot, SocketSlashCommand command)
        {
            await command.DeferAsync(true);

            string stringId = (string)command.Data.Options.First().Value;

            EmbedBuilder builder = new EmbedBuilder().WithFooter($"Searched ID: '{stringId}'");

            ulong id;
            SocketGuildUser user;

            if (ulong.TryParse(stringId, out id))
            {
                builder
                .WithAuthor($"Error!")
                .WithTitle("That is not a valid user ID!")
                .WithColor(Color.Red);
            } else if ((user = bot.Client.GetGuild(command.GuildId.Value).GetUser(id)) == null)
            {
                builder
                .WithAuthor($"Error!")
                .WithTitle("That user does not exist!")
                .WithColor(Color.Red);
            } else
            {
                builder
                .WithAuthor($"That user is {command.User.Username}#{command.User.Discriminator}")
                .WithTitle("Truth or Dare")
                .WithColor(Color.Blue);
            }

            await command.ModifyOriginalResponseAsync(p =>
            {
                p.Embed = builder.Build();
            });
        }
    }
}
