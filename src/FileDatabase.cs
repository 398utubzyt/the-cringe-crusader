using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crusader
{
    public delegate T StringParser<T>(string data) where T : notnull;
    public delegate string StringConverter<T>(T data) where T : notnull;

    /// <summary>A class for storing data in persistent storage.</summary>
    /// <typeparam name="T">Arbitrary data type.</typeparam>
    public class FileDatabase<T> : IDumpable, ICollection<T> where T : notnull
    {
        private readonly string _path;
        private T[] _db;
        private int _c;

        public T this[int index] { get { return _db[index]; } set { _db[index] = value; } }

        /// <summary>The path to the file of the <see cref="FileDatabase{T}"/>.</summary>
        public string Path => _path;
        public int Count => _c;
        /// <summary>Gets the maximum amount of elements allowed in the <see cref="FileDatabase{T}"/>.</summary>
        public int Capacity => _db.Length;

        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)_db.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _db.GetEnumerator();

        /// <summary>Gets a random <typeparamref name="T"/> from the database.</summary>
        /// <returns>A random <typeparamref name="T"/>.</returns>
        public T Random()
            => _db[(int)(System.Random.Shared.NextDouble() * _db.Length)];

        #region Dump

        /// <summary>Writes class data to persistent storage.</summary>
        /// <remarks>The exact writing location is left to the developer.</remarks>
        public void Dump()
        {
            using StreamWriter writer = File.CreateText(_path);
            for (int i = 0; i < _c; i++)
                writer.WriteLineAsync(_db[i].ToString());
        }

        /// <summary>Writes class data to persistent storage.</summary>
        /// <remarks>The exact writing location is left to the developer.</remarks>
        public void Dump(StringConverter<T> converter)
        {
            if (converter == null)
                return;
            using StreamWriter writer = File.CreateText(_path);
            for (int i = 0; i < _c; i++)
                writer.WriteLineAsync(converter(_db[i]));
        }

        /// <summary>Writes class data to persistent storage.</summary>
        /// <remarks>The exact writing location is left to the developer.</remarks>
        public async Task DumpAsync()
        {
            using StreamWriter writer = File.CreateText(_path);
            for (int i = 0; i < _c; i++)
                await writer.WriteLineAsync(_db[i].ToString());
        }

        /// <summary>Writes class data to persistent storage.</summary>
        /// <remarks>The exact writing location is left to the developer.</remarks>
        public async Task DumpAsync(StringConverter<T> converter)
        {
            if (converter == null)
                return;
            using StreamWriter writer = File.CreateText(_path);
            for (int i = 0; i < _c; i++)
                await writer.WriteLineAsync(converter(_db[i]));
        }

        #endregion

        #region IDumpable

        Task IDumpable.Dump() => DumpAsync();

        #endregion

        #region ICollection<T>

        public void Add(T item)
        {
            if (_c++ >= _db.Length)
                Array.Resize(ref _db, _db.Length * 2);
            _db[_c - 1] = item;
        }

        public void Clear()
            => Array.Clear(_db);

        /// <summary>
        /// Searches for the specified object and returns the index of its first occurrence in the <see cref="FileDatabase{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="FileDatabase{T}"/>.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of <paramref name="item"/> in the 
        /// entire <see cref="FileDatabase{T}"/>, if found; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
            => Array.IndexOf(_db, item);

        public bool Contains(T item)
            => IndexOf(item) >= 0;

        public void CopyTo(T[] array, int arrayIndex)
            => Array.Copy(_db, 0, array, arrayIndex, array.Length - arrayIndex);

        public bool Remove(T item)
        {
            int i = IndexOf(item);
            if (i < 0)
                return false;
            _db[i] = default;
            _c--;

            if (i != _c)
                Array.Copy(_db, i + 1, _db, i, _c - i);

            if (_c < _db.Length / 4)
                Array.Resize(ref _db, _db.Length / 2);

            return true;
        }

        #endregion

        /// <summary>Creates a new <see cref="FileDatabase{T}"/>.</summary>
        /// <param name="path">The path to the file which the <see cref="FileDatabase{T}"/> will write to/read from.</param>
        /// <param name="parser">A string parser to convert text into a <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> or <paramref name="parser"/> is <see langword="null"/>.</exception>
        /// <exception cref="FileNotFoundException">If <paramref name="path"/> does not point to an existing file.</exception>
        public FileDatabase(string path, StringParser<T> parser)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (parser == null)
                throw new ArgumentNullException(nameof(parser));

            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            _path = path;
            string[] lines = File.ReadAllLines(path);
            _db = new T[FileUtil.GreaterPowOf2(lines.Length)];
            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                    _db[i] = parser(lines[i]);
            }
            
            _c = lines.Length;
        }
    }
}
