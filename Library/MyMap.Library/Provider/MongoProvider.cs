using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMap.Library.Provider
{
    public static class MongoProvider
    {
        public static MongoClient client = new MongoClient("mongodb://localhost:27017/Test");
        public static MongoDatabase db = client.GetServer().GetDatabase("Test");
    }
}
