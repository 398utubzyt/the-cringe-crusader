using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crusader.Tod
{
    public enum TodType
    {
        Truth,
        Dare
    }

    public readonly struct TodPrompt
    {
        public readonly TodType Type { init; get; }
        public readonly string Text { init; get; }
    }
}
