using MongoDB.Driver;
using MyMap.Library.ModelsAjax;
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

        public MongoProvider()
        {
            MongoClient client = new MongoClient(_mongoConnect.ConnectionString);
            if (client != null)
            {
                this.db = client.GetServer().GetDatabase(_mongoConnect.DataBase);
            }
        }

        public MongoCollection<UserAjax> Users
        {
            get
            {
                return db.GetCollection<UserAjax>("User");
            }
        }

        public MongoCollection<PointAjax> Point
        {
            get
            {
                return db.GetCollection<PointAjax>("Point");
            }
        }

        public MongoCollection<PathAjax> Path
        {
            get
            {
                return db.GetCollection<PathAjax>("Path");
            }
        }

    }
}
