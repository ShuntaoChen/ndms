using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
/* Author:Calos
 * CreateTime:2015年3月12日 14:19:55
 * */
namespace System
{
    public static class Extensions
    {
        public static String IsNullOrEmpty(this String oldString)
        {
            return oldString.IsNullOrEmpty();
        }

        public static String FormatModel<T>(this String template, T model,params object[] paramArr)
        {
            return "";
        }


        public static T Fill<T>(this T model, HttpRequestBase request) where T : new()
        {
            var t = model;
            foreach (var item in t.GetType().GetProperties())
            {
                if (request[item.Name] != null)
                {
                    try
                    {
                        var val = request[item.Name];
                        if (item.PropertyType == typeof(DateTime) || item.PropertyType == typeof(Nullable<DateTime>))
                        {
                            item.SetValue(t, Convert.ChangeType(DateTime.Parse(val), item.PropertyType), null);
                        }
                        else
                        {
                            item.SetValue(t, Convert.ChangeType(val, item.PropertyType), null);
                        }
                    }
                    catch { }
                }
            }
            return t;
        }

        public static TDto ToDto<TEntity, TDto>(this TEntity entity)
        {
            return Mapper.Map<TEntity, TDto>(entity);
        }

        public static IList<TDto> ToDtoList<TEntity, TDto>(this IList<TEntity> entityList)
        {
            return Mapper.Map<IList<TEntity>, IList<TDto>>(entityList);
        }

    }
}