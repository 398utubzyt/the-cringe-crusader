using System;
using Discord;
using Discord.Commands;
using Discord.Commands.Builders;

namespace Crusader.Commands
{
    /// <summary>Collection of utility functions relating to commands.</summary>
    public static class CommandUtil
    {
        public static MessageComponent GreenRedButtons(CringeCommand cmd, string gId, string rId, string gText, string rText)
            => new ComponentBuilder().AddRow(new ActionRowBuilder()
                .WithButton(gText, $"{cmd.Name}__{gId}", ButtonStyle.Success)
                .WithButton(rText, $"{cmd.Name}__{rId}", ButtonStyle.Danger)
                ).Build();
        public static MessageComponent GreenRedBlurpleButtons(CringeCommand cmd, string gId, string rId, string bId, string gText, string rText, string bText)
            => new ComponentBuilder().AddRow(new ActionRowBuilder()
                .WithButton(gText, $"{cmd.Name}__{gId}", ButtonStyle.Success)
                .WithButton(rText, $"{cmd.Name}__{rId}", ButtonStyle.Danger)
                .WithButton(bText, $"{cmd.Name}__{bId}", ButtonStyle.Primary)
                ).Build();

        public static MessageComponent YesNoButtons(CringeCommand cmd, string yesId, string noId)
            => GreenRedButtons(cmd, yesId, noId, "Yes", "No");
    }
}
