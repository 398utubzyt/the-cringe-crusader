using System;
using Discord;

namespace Crusader.Confess
{
    /// <summary>A confession object.</summary>
    public readonly struct Confession
    {
        /// <summary>The internal ID of the confession.</summary>
        public readonly Guid Id { init; get; }
        /// <summary>The snowflake of the confession.</summary>
        public readonly ulong Date { init; get; }
        /// <summary>The author's user ID.</summary>
        public readonly ulong Author { init; get; }
        /// <summary>The message of the confession.</summary>
        public readonly string Message { init; get; }
    }
}
