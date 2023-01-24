using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Crusader.Confess
{
    public class Confessions : IDumpable
    {
        private readonly FileDatabase<Confession>[] confessions; // This naming scheme will not get confusing...
        private readonly int current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Confession ParsePrompt(string text)
        {
            string[] args = text.Split(' ');
            return new Confession()
            {
                Id = Guid.Parse(args[0]),
                Date = ulong.Parse(args[1]),
                Author = ulong.Parse(args[2]),
                Message = string.Join(' ', args[3..]),
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ConvertPrompt(Confession prompt) => $"{prompt.Id} {prompt.Date} {prompt.Author} {prompt.Message}";

        public async Task Dump()
        {
            foreach (FileDatabase<Confession> db in confessions)
                await db.DumpAsync(ConvertPrompt);
        }

        public Confession Create(ulong date, ulong author, string message)
        {
            Confession confession = new Confession() { Id = Guid.NewGuid(), Date = date, Author = author, Message = message };
            confessions[current].Add(confession);
            return confession;
        }

        public Confession Get(Guid id)
        {
            for (int i = 0; i < confessions.Length; i++)
            {
                for (int j = 0; j < confessions[i].Count; j++)
                {
                    if (confessions[i][j].Id == id)
                        return confessions[i][j];
                }
            }

            return new Confession() { Message = $"No confession exists with id {id} exists." };
        }

        public Confessions()
        {
            current = 0;
            while (FileUtil.Ensure(FileUtil.Root($"confessions/{current}.txt")))
                current++;

            confessions = new FileDatabase<Confession>[current + 1];
            for (int i = 0; i < confessions.Length; i++)
                confessions[i] = new FileDatabase<Confession>(FileUtil.Root($"confessions/{i}.txt"), ParsePrompt);
        }
    }
}
