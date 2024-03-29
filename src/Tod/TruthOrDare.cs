﻿using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Crusader.Tod
{
    public class TruthOrDare : IDumpable
    {
        private readonly FileDatabase<TodPrompt> truths;
        private readonly FileDatabase<TodPrompt> dares;
        private readonly FileDatabase<TodPrompt> wyrs;

        private int lastTruth;
        private int lastDare;
        private int lastWyr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TodPrompt ParsePromptTruth(string text) => new TodPrompt() { Type = TodType.Truth, Text = text };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TodPrompt ParsePromptDare(string text) => new TodPrompt() { Type = TodType.Dare, Text = text };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TodPrompt ParsePromptWyr(string text) => new TodPrompt() { Type = TodType.Wyr, Text = text };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ConvertPrompt(TodPrompt prompt) => prompt.Text;

        /// <summary>Returns an index that will never equal the parameter <paramref name="prev"/>.</summary>
        /// <returns>The index which never equals <paramref name="prev"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int EnsureDifferent(double random, int count, int prev)
            => prev >= 0 ? ((count = (int)(random * (count - 1))) < prev ? count : count + 1) : (int)(random * count);

        /// <summary>Gets a random Truth prompt.</summary>
        /// <returns>A random truth prompt.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetTruth() => truths[lastTruth = EnsureDifferent(Random.Shared.NextDouble(), truths.Count, lastTruth)];

        /// <summary>Gets a random Dare prompt.</summary>
        /// <returns>A random dare prompt.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetDare() => dares[lastDare = EnsureDifferent(Random.Shared.NextDouble(), dares.Count, lastDare)];

        /// <summary>Gets a random Would You Rather prompt.</summary>
        /// <returns>A random Would You Rather prompt.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetWyr() => wyrs[lastWyr = EnsureDifferent(Random.Shared.NextDouble(), wyrs.Count, lastWyr)];

        /// <summary>Gets a random Truth or Dare prompt.</summary>
        /// <returns>A random prompt.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TodPrompt GetTruthOrDare() => Random.Shared.NextDouble() < 0.5 ? GetTruth() : GetDare();

        /// <summary>Adds a prompt to the database.</summary>
        /// <param name="prompt">The prompt to add.</param>
        public void Add(TodPrompt prompt)
        {
            switch (prompt.Type)
            {
                case TodType.Truth:
                    truths.Add(prompt);
                    break;

                case TodType.Dare:
                    dares.Add(prompt);
                    break;

                case TodType.Wyr:
                    wyrs.Add(prompt);
                    break;
            }
        }

        public async Task Dump()
        {
            await truths.DumpAsync(ConvertPrompt);
            await dares.DumpAsync(ConvertPrompt);
            await wyrs.DumpAsync(ConvertPrompt);
        }

        /// <summary>Creates a new Truth or Dare instance.</summary>
        public TruthOrDare()
        {
            FileUtil.Ensure(FileUtil.Root("truth.txt"));
            FileUtil.Ensure(FileUtil.Root("dare.txt"));
            FileUtil.Ensure(FileUtil.Root("wyr.txt"));

            truths = new FileDatabase<TodPrompt>(FileUtil.Root("truth.txt"), ParsePromptTruth);
            dares = new FileDatabase<TodPrompt>(FileUtil.Root("dare.txt"), ParsePromptDare);
            wyrs = new FileDatabase<TodPrompt>(FileUtil.Root("wyr.txt"), ParsePromptWyr);

            lastTruth = -1;
            lastDare = -1;
            lastWyr = -1;
        }
    }
}
