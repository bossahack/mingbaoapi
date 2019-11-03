
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Book.Dal.Model
{


    /// <summary>
    /// Uses the Name value of the <see cref="ColumnAttribute"/> specified to determine
    /// the association between the name of the column in the query results and the member to
    /// which it will be extracted. If no column mapping is present all members are mapped as
    /// usual.
    /// </summary>
    /// <typeparam name="T">The type of the object that this association between the mapper applies to.</typeparam>
    public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
    {
        public ColumnAttributeTypeMapper()
            : base(new SqlMapper.ITypeMap[]
                {
                    new CustomPropertyTypeMap(
                       typeof(T),
                       (type, columnName) =>
                           type.GetProperties().FirstOrDefault(prop =>
                               prop.GetCustomAttributes(false)
                                   .OfType<ColumnAttribute>()
                                   .Any(attr => attr.Name == columnName)
                               )
                       ),
                    new DefaultTypeMap(typeof(T))
                })
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            _mappers = mappers;
        }


        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    ConstructorInfo result = mapper.FindConstructor(names, types);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetConstructorParameter(constructor, columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }


        public ConstructorInfo FindExplicitConstructor()
        {
            return _mappers
                .Select(mapper => mapper.FindExplicitConstructor())
                .FirstOrDefault(result => result != null);
        }
    }


    public class ColumnMapper
    {
        public static void SetMapper()
        {
            //数据库字段名和c#属性名不一致，手动添加映射关系
            SqlMapper.SetTypeMap(typeof(BOrder), new ColumnAttributeTypeMapper<BOrder>());
            SqlMapper.SetTypeMap(typeof(BOrderAbnormal), new ColumnAttributeTypeMapper<BOrderAbnormal>());
            SqlMapper.SetTypeMap(typeof(BOrderItem), new ColumnAttributeTypeMapper<BOrderItem>());
            SqlMapper.SetTypeMap(typeof(Food), new ColumnAttributeTypeMapper<Food>());
            SqlMapper.SetTypeMap(typeof(Shop), new ColumnAttributeTypeMapper<Shop>());
            SqlMapper.SetTypeMap(typeof(ShopOrderDate), new ColumnAttributeTypeMapper<ShopOrderDate>());
            SqlMapper.SetTypeMap(typeof(UserInfo), new ColumnAttributeTypeMapper<UserInfo>());
            SqlMapper.SetTypeMap(typeof(UserShop), new ColumnAttributeTypeMapper<UserShop>());
            SqlMapper.SetTypeMap(typeof(ShopOnline), new ColumnAttributeTypeMapper<ShopOnline>());
            SqlMapper.SetTypeMap(typeof(ShopDayOrder), new ColumnAttributeTypeMapper<ShopDayOrder>());

            //每个需要用到[colmun(Name="")]特性的model，都要在这里添加映射
        }
    }
}
