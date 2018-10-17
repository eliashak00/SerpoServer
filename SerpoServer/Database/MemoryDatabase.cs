using System.IO;
using LiteDB;

namespace SerpoServer.Database
{
    public class MemoryDatabase
    {
        public readonly LiteDatabase Current;

        private volatile MemoryStream _memoryContent = new MemoryStream();

        public MemoryDatabase()
        {
            Current = new LiteDatabase(_memoryContent);
        }

        ~MemoryDatabase()
        {
            Current.Dispose();
        }
    }
}