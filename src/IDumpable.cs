using System.Threading.Tasks;

namespace Crusader
{
    /// <summary>
    /// Defines a method for writing data to persistent storage.
    /// </summary>
    public interface IDumpable
    {
        /// <summary>Writes class data to persistent storage.</summary>
        /// <remarks>The exact writing location is left to the developer.</remarks>
        public Task Dump();
    }
}
