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

    public struct TodPrompt
    {
        public TodType Type;
        public string Text;
    }
}
