using Book.Dal.Model;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Book.Model.Enums;
using System.Text;

namespace Book.Dal
{
    public class FoodDal
    {
        private static FoodDal _Instance;
        public static FoodDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new FoodDal();
            return _Instance;
        }

        public List<Food> GetList(int shopid)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<Food>("SELECT * FROM food where shop_id=@shopid and status<>@status", new { shopid = shopid, status = FoodStatus.Removed }).ToList();
            }
        }

        public List<Food> GetList(int type, int shopid)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<Food>("SELECT * FROM food where type=@type AND shop_id=@shopid and status<>@status", new { shopid = shopid, type = type, status = FoodStatus.Removed }).ToList();
            }
        }

        public List<Food> GetList(List<int> ids)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<Food>("SELECT * FROM food where id in @ids",new { ids=ids}).ToList();
            }
        }

        public Food Get(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Get<Food>(id);
            }
        }

        public void Add(Food food)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Insert<Food>(food);
            }
        }

        public void edit(Food food)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Update<Food>(food);
            }
        }

        public void enable(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE food SET `status`=@status WHERE id=@id",new { id = id, status = FoodStatus.Normal });
            }
        }

        public void disable(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE food SET `status`=@status WHERE id=@id", new { id = id, status = FoodStatus.Offshelved });
            }
        }

        public void Remove(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE food SET `status`=@status WHERE id=@id", new { id = id, status = FoodStatus.Removed });
            }
        }

        public bool existType(int foodtype)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result=conn.ExecuteScalar("SELECT 1 FROM food where type =@type", new { type=foodtype});
                if (result != null)
                    return true;
                return false;
            }
        }


        public void FoodReorder(List<Food> foods)
        {
            StringBuilder sb = new StringBuilder();
            using (var conn = SqlHelper.GetInstance())
            {
                foods.ForEach(food =>
                {
                    sb.Append($"update food set `Level`={food.Level} where id={food.Id};");
                });
                conn.Execute(sb.ToString());
            }
        }
    }
}
