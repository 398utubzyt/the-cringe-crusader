using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crusader
{
    public delegate T StringParser<T>(string data) where T : notnull;
    public delegate string StringConverter<T>(T data) where T : notnull;

    public class FileDatabase<T> : ICollection<T> where T : notnull
    {
        private readonly string _path;
        private T[] _db;
        private int _c;

        public T this[int index] { get { return _db[index]; } set { _db[index] = value; } }

        public string Path => _path;
        public int Count => _c;
        public int Size => _db.Length;

        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator() => (IEnumerator<T>)_db.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _db.GetEnumerator();

        public T Random()
            => _db[(int)(System.Random.Shared.NextDouble() * _db.Length)];

        public void Dump()
        {
            using StreamWriter writer = File.CreateText(_path);
            foreach (T t in _db)
                writer.WriteLine(t.ToString());
        }

        public void Dump(StringConverter<T> converter)
        {
            if (converter == null)
                return;
            using StreamWriter writer = File.CreateText(_path);
            foreach (T t in _db)
                writer.WriteLine(converter(t));
        }

        #region ICollection<T>

        public void Add(T item)
        {
            if (_c++ >= _db.Length)
                Array.Resize(ref _db, _db.Length * 2);
            _db[_c - 1] = item;
        }

        public void Clear()
            => Array.Clear(_db);

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
                _db[i] = parser(lines[i]);
            _c = lines.Length;
        }
    }
}
