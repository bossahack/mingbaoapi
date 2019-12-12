using Book.Dal.Model;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Book.Model;
using System.Text;

namespace Book.Dal
{
    public class FoodTypeDal
    {
        private static FoodTypeDal _Instance;
        public static FoodTypeDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new FoodTypeDal();
            return _Instance;
        }

        public void Add(FoodType foodtype)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Insert<FoodType>(foodtype);
            }
        }

        public void Edit(int id,string name)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE food_type set `name`=@name WHERE id=@id", new { name = name, id = id });
            }
        }

        public void remove(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Delete<FoodType>(id);
            }
        }

        public List<FoodType> GetList(int shopid)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<FoodType>("SELECT * FROM food_type where shop_id=@shopid", new { shopid=shopid})?.ToList();
            }
        }

        public FoodType Get(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Get<FoodType>(id);
            }
        }

        public bool ExistName(int shopid,string name)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.QueryFirstOrDefault<FoodType>("SELECT * FROM food_type where shop_id=@shopid and name=@name",new { shopid=shopid, name=name })==null?false:true;
            }
        }

        public void TypeReorder(List<FoodTypeResponse> types)
        {
            StringBuilder sb = new StringBuilder();
            using (var conn = SqlHelper.GetInstance())
            {
                types.ForEach(type =>
                {
                    sb.Append($"update food_type set `Level`={type.Level} where id={type.Id};");
                });
                conn.Execute(sb.ToString());
            }
        }

        public int GetCount(int shopId)
        {
            using (var conn = SqlHelper.GetInstance())
            {               
                return conn.ExecuteScalar<int>("select COUNT(1) from food_type WHERE shop_id=@shopId",new { shopId});
            }
        }
    }
}
