using MongoDB.Driver;
using MyMap.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMap.Library.Provider
{

    public  class MongoProvider
    {
        MongoConnection _mongoConnect = new MongoConnection();
        public MongoDatabase db = null;

        [Obsolete]
        public MongoProvider()
        {
            MongoClient client = new MongoClient(_mongoConnect.ConnectionString);
            if (client != null)
            {
                this.db = client.GetServer().GetDatabase(_mongoConnect.DataBase);
            }
        }

        public MongoCollection<User> Users
        {
            get
            {
                return db.GetCollection<User>("User");
            }
        }
    }
}
