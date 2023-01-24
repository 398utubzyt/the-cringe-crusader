using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;
using System.Linq;

using Crusader.Confess;

namespace Crusader.Commands
{
    public class QueryConfessionCommand : CringeCommand
    {
        public override string Name => "cquery";
        public override string Description => "Look at a confession's data.";
        public override GuildPermission Permission => GuildPermission.ManageGuild;

        public override Task Build(SlashCommandBuilder builder)
        {
            builder.AddOption(
                new SlashCommandOptionBuilder()
                .WithName("id")
                .WithDescription("The ID of the confession to query.")
                .WithType(ApplicationCommandOptionType.String)
                .WithRequired(true)
            );
            return Task.CompletedTask;
        }

        public override async Task Run(Bot bot, SocketSlashCommand command)
        {
            await command.DeferAsync(true);

            string stringId = (string)command.Data.Options.First().Value;

            EmbedBuilder builder = new EmbedBuilder();

            Guid id;
            Confession confession;

            if (Guid.TryParse(stringId, out id))
            {
                builder
                .WithAuthor($"Error!")
                .WithTitle("That is not a valid confession ID!")
                .WithColor(Color.Red);
            } else if ((confession = bot.Confessions.Get(id)).Author == 0)
            {
                builder
                .WithAuthor($"Error!")
                .WithTitle("That confession does not exist!")
                .WithColor(Color.Red);
            } else
            {
                SocketGuildUser gu;
                IUser user;
                if ((gu = bot.Client.GetGuild(command.GuildId.Value).GetUser(confession.Author)) != null)
                    builder.WithAuthor($"{gu.Username}#{gu.Discriminator} ({gu.Nickname})", gu.GetAvatarUrl());
                else if ((user = await bot.Client.GetUserAsync(confession.Author)) != null)
                    builder.WithAuthor($"{user.Username}#{user.Discriminator}", user.GetAvatarUrl());
                else
                    builder.WithAuthor($"Unknown User", bot.Client.CurrentUser.GetDefaultAvatarUrl());

                builder
                .WithTitle($"\"{confession.Message}\"")
                .WithFooter($"Sent at {SnowflakeUtils.FromSnowflake(confession.Date).LocalDateTime} - Confession ID {confession.Id}")
                .WithColor(Color.Blue);
            }

            await command.ModifyOriginalResponseAsync(p =>
            {
                p.Embed = builder.Build();
            });
        }
    }
}
