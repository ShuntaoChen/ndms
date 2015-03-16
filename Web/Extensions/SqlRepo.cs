using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*
 * sentence repository
 * */
namespace System
{
    public sealed class SqlRepo : Dictionary<String, String>
    {
        private static readonly SqlRepo _instance = new SqlRepo();
        public static  SqlRepo Instance
        {
            get
            {
                return _instance;
            }
        }
        public new String this[String key]
        {
            get
            {
                return _instance[key];
            }
        }
        private SqlRepo()
        {
            this.Add("", "Select * from ");
        }
        

    }
}