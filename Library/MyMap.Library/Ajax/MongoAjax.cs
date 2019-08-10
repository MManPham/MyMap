using AjaxPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMap.Library.Provider;
using System.Threading.Tasks;

namespace MyMap.Library.Ajax
{
    public class MinhMan
    {
        public string Name { get; set; } 
    }
    public class MongoAjax
    {
        [AjaxMethod]
        public bool SaveFavoriteRoute(string path)
        {
            var a = MongoProvider.db.GetCollection<MinhMan>("Favorite").Insert(new MinhMan { Name = "Man Xau Tra"}) ;
            return true;
        }
    }
}
