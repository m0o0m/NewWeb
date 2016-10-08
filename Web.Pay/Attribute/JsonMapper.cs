using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Web.Pay
{
    namespace JsonMapper
    {
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class JsonFieldAttribute : Attribute
        {
            private string _Name = string.Empty;

            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
        }
    }

    namespace JsonMapper
    {
        public class JsonToInstance
        {
            public T ToInstance<T>(string json) where T : new()
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string[] fields = json.Replace("{", "").Replace("}", "").Split(',');
                for (int i = 0; i < fields.Length; i++)
                {
                    string[] keyvalue = fields[i].Split(':');
                    dic.Add(Filter(keyvalue[0]), Filter(keyvalue[1]));
                }

                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                T entity = new T();
                foreach (PropertyInfo property in properties)
                {
                    object[] propertyAttrs = property.GetCustomAttributes(false);
                    for (int i = 0; i < propertyAttrs.Length; i++)
                    {
                        object propertyAttr = propertyAttrs[i];
                        if (propertyAttr is JsonFieldAttribute)
                        {
                            JsonFieldAttribute jsonFieldAttribute = propertyAttr as JsonFieldAttribute;
                            foreach (KeyValuePair<string, string> item in dic)
                            {
                                if (item.Key == jsonFieldAttribute.Name)
                                {
                                    Type t = property.PropertyType;
                                    property.SetValue(entity, ToType(t, item.Value), null);
                                    break;
                                }
                            }
                        }
                    }
                }
                return entity;
            }

            private string Filter(string str)
            {
                if (!(str.StartsWith("\"") && str.EndsWith("\"")))
                {
                    return str;
                }
                else
                {
                    return str.Substring(1, str.Length - 2);
                }
            }

            public object ToType(Type type, string value)
            {
                if (type == typeof(string))
                {
                    return value;
                }

                MethodInfo parseMethod = null;

                foreach (MethodInfo mi in type.GetMethods(BindingFlags.Static
                    | BindingFlags.Public))
                {
                    if (mi.Name == "Parse" && mi.GetParameters().Length == 1)
                    {
                        parseMethod = mi;
                        break;
                    }
                }

                if (parseMethod == null)
                {
                    throw new ArgumentException(string.Format(
                        "Type: {0} has not Parse static method!", type));
                }

                return parseMethod.Invoke(null, new object[] { value });
            }
        }
    }
}