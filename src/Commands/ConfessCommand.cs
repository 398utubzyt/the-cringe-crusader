using Discord;
using Discord.WebSocket;

using System;
using System.Linq;
using System.Threading.Tasks;

using Crusader.Confess;

namespace Crusader.Commands
{
    /// <summary>/confess</summary>
    public class ConfessCommand : CringeCommand
    {
        public override string Name => "confess";
        public override string Description => "Anonymously confess something!";

        public override Task Build(SlashCommandBuilder builder)
        {
            builder.AddOption("content", ApplicationCommandOptionType.String, "The message to confess.", true);

            return Task.CompletedTask;
        }

        public override async Task Run(Bot bot, SocketSlashCommand command)
        {
            await command.DeferAsync(true);

            Confession confession = bot.Confessions.Create(SnowflakeUtils.ToSnowflake(DateTimeOffset.Now), command.User.Id, 
                command.Data.Options.First().Value.ToString());

            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle("Anonymous Confession")
                .WithColor(new Color(
                    -Random.Shared.NextSingle() * 0.5f + 1.0f,
                    -Random.Shared.NextSingle() * 0.5f + 1.0f,
                    -Random.Shared.NextSingle() * 0.5f + 1.0f
                    ))
                .WithFields(new EmbedFieldBuilder().WithName($"\"{confession.Message}\""))
                .WithFooter(confession.Id.ToString());

            await command.RespondAsync(null, null, false, false, null, null, builder.Build());

            await command.ModifyOriginalResponseAsync(p =>
            {
                p.Content = "Confession was sent! :thumbsup:";
            });
        }
    }
}
