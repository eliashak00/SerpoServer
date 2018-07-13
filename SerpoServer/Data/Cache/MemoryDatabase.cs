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
    public static class MemoryDatabase
    {
        public static LiteDatabase Current = new LiteDatabase(new MemoryStream());


        public static LiteCollection<T> GetCollectionByType<T>()
        {
            var coll = Current.GetCollection<T>(typeof(T).Name + "cache");
            return coll.Count() < 0 ? null : coll;
        }
    }
}