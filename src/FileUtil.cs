using System;
using System.IO;

namespace Crusader
{
    public static class FileUtil
    {
        public static int GreaterPowOf2(int value)
        {
            int x;
            for (int i = 0; i < sizeof(int) * 8; i++)
            {
                x = 1 << i;
                if (x > value)
                    return x;
            }
            return value;
        }

        public static void Ensure(string path)
        {
            if (!File.Exists(path))
                File.Create(path).Close();
        }

        public static string Root(string path)
            => Path.Join(Path.GetDirectoryName(Environment.ProcessPath), path);

        public static string Combine(string path1, string path2)
            => Path.Join(path1, path2);
        public static string Combine(string path1, string path2, string path3)
            => Path.Join(path1, path2, path3);
        public static string Combine(string path1, string path2, string path3, string path4)
            => Path.Join(path1, path2, path3, path4);
        public static string Combine(params string[] paths)
            => Path.Join(paths);
    }
}
