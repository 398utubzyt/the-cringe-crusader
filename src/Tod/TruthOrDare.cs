using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Crusader.Tod
{
    public class TruthOrDare : IDumpable
    {
        private readonly FileDatabase<TodPrompt> truths;
        private readonly FileDatabase<TodPrompt> dares;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TodPrompt ParsePromptTruth(string text) => new TodPrompt() { Type = TodType.Truth, Text = text };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TodPrompt ParsePromptDare(string text) => new TodPrompt() { Type = TodType.Dare, Text = text };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ConvertPrompt(TodPrompt prompt) => prompt.Text;

        /// <summary>Gets a random Truth prompt.</summary>
        /// <returns>A random truth prompt.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetTruth() => truths[(int)(Random.Shared.NextDouble() * truths.Count)];

        /// <summary>Gets a random Dare prompt.</summary>
        /// <returns>A random dare prompt.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetDare() => dares[(int)(Random.Shared.NextDouble() * dares.Count)];

        /// <summary>Gets a random Truth or Dare prompt.</summary>
        /// <returns>A random prompt.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetRandom() => Random.Shared.NextDouble() < 0.5 ? GetTruth() : GetDare();

        public async Task Dump()
        {
            await truths.DumpAsync(ConvertPrompt);
            await dares.DumpAsync(ConvertPrompt);
        }

        /// <summary>Creates a new Truth or Dare instance.</summary>
        public TruthOrDare()
        {
            FileUtil.Ensure(FileUtil.Root("truth.txt"));
            FileUtil.Ensure(FileUtil.Root("dare.txt"));

            truths = new FileDatabase<TodPrompt>(FileUtil.Root("truth.txt"), ParsePromptTruth);
            dares = new FileDatabase<TodPrompt>(FileUtil.Root("dare.txt"), ParsePromptDare);
        }
    }
}
