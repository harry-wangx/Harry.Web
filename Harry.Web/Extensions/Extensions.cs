using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Web.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// 获取<see cref="SelectListItem"/>集合
        /// <para>Text=IDictionary.Key,Value=IDictionary.Value</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectList(this IDictionary<string, string> source)
        {
            if (source == null || source.Count <= 0)
            {
                yield break;
            }
            int i = 0;
            foreach (var item in source)
            {
                yield return new SelectListItem()
                {
                    Value = item.Value,
                    Text = item.Key,
                    Selected = i == 0
                };
                i++;
            }
        }
    }
}
