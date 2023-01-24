using System;
using System.Runtime.CompilerServices;

namespace Crusader.Tod
{
    public class TruthOrDare
    {
        private FileDatabase<TodPrompt> truths;
        private FileDatabase<TodPrompt> dares;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TodPrompt ParsePrompt(string text) => new TodPrompt()
        {
                Type = text.ToLower()[0] switch { 't' => TodType.Truth, 'd' => TodType.Dare, _ => TodType.Truth },
                Text = text[1..]
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ConvertPrompt(TodPrompt prompt) => $"{(prompt.Type == TodType.Dare ? 'd' : 't')}{prompt.Text}"

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetTruth() => truths[(int)(Random.Shared.NextDouble() * truths.Count)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetDare() => dares[(int)(Random.Shared.NextDouble() * dares.Count)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetRandom() => Random.Shared.NextDouble() < 0.5 ? GetTruth() : GetDare();

        public TruthOrDare()
        {
            FileUtil.Ensure(FileUtil.Root("truth.txt"));
            FileUtil.Ensure(FileUtil.Root("dare.txt"));

            truths = new FileDatabase<TodPrompt>(FileUtil.Root("truth.txt"), ParsePrompt);
            dares = new FileDatabase<TodPrompt>(FileUtil.Root("dare.txt"), ParsePrompt);
        }
    }
}
