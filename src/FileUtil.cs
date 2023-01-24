using System;
using System.IO;

namespace Crusader
{
    /// <summary>Methods to help with file management.</summary>
    public static class FileUtil
    {
        /// <summary>Gets the next power of 2 which is greater than <paramref name="value"/>.</summary>
        /// <param name="value">The threshold.</param>
        /// <returns>The next power of 2.</returns>
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

        /// <summary>Gets whether a file <paramref name="path"/> exists or not.</summary>
        /// <param name="path">The path to check.</param>
        /// <returns><see langword="true"/> if it already exists, otherwise <see langword="false"/>.</returns>
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>Ensures that a file <paramref name="path"/> exists.</summary>
        /// <param name="path">The path to check.</param>
        /// <returns><see langword="true"/> if it already exists, otherwise <see langword="false"/>.</returns>
        public static bool Ensure(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                return false;
            }
            return true;
        }

        /// <summary>Combines <paramref name="path"/> with the application directory.</summary>
        /// <param name="path">The path to combine.</param>
        /// <returns>The combined result of the full application directory and <paramref name="path"/>.</returns>
        public static string Root(string path)
            => Path.Join(Path.GetDirectoryName(Environment.ProcessPath), path);

        /// <summary>Concatenates two paths.</summary>
        /// <returns>The combined path.</returns>
        public static string Combine(string path1, string path2)
            => Path.Join(path1, path2);
        /// <summary>Concatenates three paths.</summary>
        /// <returns>The combined path.</returns>
        public static string Combine(string path1, string path2, string path3)
            => Path.Join(path1, path2, path3);
        /// <summary>Concatenates four paths.</summary>
        /// <returns>The combined path.</returns>
        public static string Combine(string path1, string path2, string path3, string path4)
            => Path.Join(path1, path2, path3, path4);
        /// <summary>Concatenates a collection of paths.</summary>
        /// <returns>The combined path.</returns>
        public static string Combine(params string[] paths)
            => Path.Join(paths);
    }
}
