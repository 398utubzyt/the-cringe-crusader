using System;
using Discord;

namespace Crusader.Confess
{
    public readonly struct Confession
    {
        public readonly Guid Id { init; get; }
        public readonly ulong Date { init; get; }
        public readonly ulong Author { init; get; }
        public readonly string Message { init; get; }
    }
}
