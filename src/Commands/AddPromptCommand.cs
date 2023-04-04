using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;
using System.Linq;
using Crusader.Tod;

namespace Crusader.Commands
{
    /// <summary>/addprompt</summary>
    public class AddPromptCommand : CringeCommand
    {
        public override string Name => "addprompt";
        public override string Description => "Creates a new user-generated Truth or Dare prompt.";
        public override GuildPermission Permission => GuildPermission.ManageGuild;

        public override Task Build(SlashCommandBuilder builder)
        {
            builder.AddOption(
                new SlashCommandOptionBuilder()
                .WithName("type")
                .WithDescription("The type of prompt being created.")
                .WithType(ApplicationCommandOptionType.Integer)
                .WithRequired(true)
                .AddChoice("truth", 1)
                .AddChoice("dare", 2)
                .AddChoice("wyr", 3)
            ).AddOption(
                new SlashCommandOptionBuilder()
                .WithName("prompt")
                .WithDescription("The text to display for the prompt.")
                .WithType(ApplicationCommandOptionType.String)
                .WithRequired(true)
            );
            return Task.CompletedTask;
        }

        public override async Task Run(Bot bot, SocketSlashCommand command)
        {
            await command.DeferAsync(true);

            SocketSlashCommandDataOption[] options = command.Data.Options.ToArray();
            bot.TruthOrDare.Add(new TodPrompt() { Type = (TodType)(long)options[0].Value, Text = (string)options[1].Value });

            EmbedBuilder builder = new EmbedBuilder();

            builder
                .WithAuthor($"Success")
                .WithTitle($"Added new {(TodType)(long)options[0].Value} prompt!")
                .WithDescription($"\"{(string)options[1].Value}\"")
                .WithColor(Color.Green);

            await command.ModifyOriginalResponseAsync(p =>
            {
                p.Embed = builder.Build();
            });
        }
    }
}
