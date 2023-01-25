using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusader.Tod
{
    /// <summary>
    /// Truth or Dare mode.
    /// </summary>
    public enum TodType
    {
        /// <summary>Truth.</summary>
        Truth = 1,
        /// <summary>Dare.</summary>
        Dare = 2,
        /// <summary>Would You Rather.</summary>
        Wyr = 3,
    }

    public readonly struct TodPrompt
    {
        /// <summary>The type of Truth or Dare prompt.</summary>
        public readonly TodType Type { init; get; }
        /// <summary>The prompt string.</summary>
        public readonly string Text { init; get; }
    }
}
