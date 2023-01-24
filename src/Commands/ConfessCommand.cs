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

        private const float RandomColorThreshold = 0.25f;

        public override async Task Run(Bot bot, SocketSlashCommand command)
        {
            await command.DeferAsync(true);

            Confession confession = bot.Confessions.Create(SnowflakeUtils.ToSnowflake(DateTimeOffset.Now), command.User.Id, 
                command.Data.Options.First().Value.ToString());

            EmbedBuilder builder = new EmbedBuilder()
                .WithColor(new Color(
                    -Random.Shared.NextSingle() * RandomColorThreshold + 1.0f,
                    -Random.Shared.NextSingle() * RandomColorThreshold + 1.0f,
                    -Random.Shared.NextSingle() * RandomColorThreshold + 1.0f
                    ))
                .WithFields(
                    new EmbedFieldBuilder()
                        .WithName("Anonymous Confession")
                        .WithValue($"\"{confession.Message}\"")
                        .WithIsInline(true))
                .WithFooter($"ID: {confession.Id}");

            await command.Channel.SendMessageAsync(null, false, builder.Build());

            await command.ModifyOriginalResponseAsync(p =>
            {
                p.Content = "Confession was sent! :thumbsup:";
            });
        }
    }
}
