// ################################################
// ##@project SerpoCMS.Core
// ##@filename MemoryDatabase.cs
// ##@author Elias Håkansson
// ##@license MIT - License(see license.txt)
// ################################################

using System.IO;
using LiteDB;

namespace SerpoServer.Data.Cache
{
    public class MemoryDatabase
    {
        private static LiteDatabase db;

        public static LiteDatabase Current
        {
            get
            {
                if (db != null) return db;
                var memStream = new MemoryStream();
                db = new LiteDatabase(memStream);
                return db;
            }
            private set => db = value;
        }

        public static LiteCollection<T> GetCollectionByType<T>()
        {
            var coll = Current.GetCollection<T>(typeof(T).Name + "cache");
            return coll.Count() < 0 ? null : coll;
        }
    }
}