using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Crusader.Confess
{
    public class Confessions : IDumpable
    {
        private readonly FileDatabase<Confession>[] confessions; // This naming scheme will not get confusing...
        private readonly int current;

        /// <summary>Get total confession count.</summary>
        public int Total { get 
            { 
                int result = 0;
                foreach (FileDatabase<Confession> db in confessions)
                    if (db != null) result += db.Count;
                return result; 
            } }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Confession ParsePrompt(string text)
        {
            string[] args = text.Split(' ', 4);
            return new Confession()
            {
                Id = Guid.Parse(args[0]),
                Date = ulong.Parse(args[1]),
                Author = ulong.Parse(args[2]),
                Message = string.Join(' ', args[3]),
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ConvertPrompt(Confession prompt) => $"{prompt.Id} {prompt.Date} {prompt.Author} {prompt.Message}";

        public async Task Dump()
        {
            // Do not dump if there is no "current" database.
            if (confessions[current] != null && confessions[current].Count > 0)
                await confessions[current].DumpAsync(ConvertPrompt);
        }

        /// <summary>Creates a new confession.</summary>
        /// <param name="date">The date snowflake.</param>
        /// <param name="author">The user id.</param>
        /// <param name="message">The message to send.</param>
        /// <returns>A Confession struct with the necessary data required to function.</returns>
        public Confession Create(ulong date, ulong author, string message)
        {
            // If no database yet, do a bit of file stuff.
            if (confessions[current] == null)
            {
                FileUtil.Ensure(FileUtil.Root($"confessions/{current}.txt"));
                confessions[current] = new FileDatabase<Confession>(FileUtil.Root($"confessions/{current}.txt"), ParsePrompt);
            }

            // Registers the command to the database and returns it.
            Confession confession = new Confession() { Id = Guid.NewGuid(), Date = date, Author = author, Message = message };
            confessions[current].Add(confession);
            return confession;
        }

        /// <summary>Gets a confession with the specified <paramref name="id"/>.</summary>
        /// <param name="id">The id of the confession.</param>
        /// <returns>If the confession exists, the confession. Otherwise, a confession with an ID of <see cref="Guid.Empty"/>.</returns>
        public Confession Get(Guid id)
        {
            for (int i = 0; i < confessions.Length; i++)
            {
                for (int j = 0; confessions[i] != null && j < confessions[i].Count; j++)
                {
                    if (confessions[i][j].Id == id)
                        return confessions[i][j];
                }
            }

            return new Confession() { Message = $"No confession exists with id {id} exists." };
        }

        /// <summary>Creates a new Confessions instance.</summary>
        public Confessions()
        {
            current = 0;
            while (FileUtil.Exists(FileUtil.Root($"confessions/{current}.txt")))
                current++; // Do not automatically create.

            confessions = new FileDatabase<Confession>[current + 1];

            // Ignore "current" database, confessions may not be
            // created and therefore creates unnecessary files.
            for (int i = 0; i < confessions.Length - 1; i++)
                confessions[i] = new FileDatabase<Confession>(FileUtil.Root($"confessions/{i}.txt"), ParsePrompt);
        }
    }
}
