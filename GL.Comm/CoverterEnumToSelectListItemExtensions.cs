using GL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 在MVC开发中我们常常用到枚举类型，通常枚举类型在使用中是是用DropDownList，每次转换不是什么好办法。 通过扩展加以实现此功能。
    /// </summary>
    public static class CoverterEnumToSelectListItemExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem(this Enum valueEnum)
        {
            var values = Enum.GetValues(valueEnum.GetType());
            var result = from int value in values select new SelectListItem { Text = Enum.GetName(valueEnum.GetType(), value), Value = value.ToString() };
            return result;
        }
        public static List<SelectListItem> ToSelectListItem(this Enum valueEnum, string selectName)
        {
            return (from int value in Enum.GetValues(valueEnum.GetType())
                    select new SelectListItem
                    {
                        Text = Enum.GetName(valueEnum.GetType(), value),
                        Value = Enum.GetName(valueEnum.GetType(), value),
                        Selected = Enum.GetName(valueEnum.GetType(), value) == selectName ? true : false
                    }).ToList();
        }
    }
}
