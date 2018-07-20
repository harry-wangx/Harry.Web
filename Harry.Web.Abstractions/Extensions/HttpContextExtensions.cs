using Microsoft.AspNetCore.Http;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Harry.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetOrCreateUUID(this HttpContext HttpContext, string key = "uuid", Action<CookieOptions> cookieOptionsAction = null)
        {
            string uuid = HttpContext.Request.Cookies[key];

            if (string.IsNullOrEmpty(uuid) || !Regex.IsMatch(uuid, @"^[\w+\-]{36}$"))
            {
                uuid = Guid.NewGuid().ToString().ToUpper();
                var options = new CookieOptions();
                options.IsEssential = true;
                cookieOptionsAction?.Invoke(options);
                HttpContext.Response.Cookies.Append(key, uuid, options);
            }
            return uuid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetUUID(this HttpContext HttpContext, string key = "uuid")
        {
            return HttpContext.Request.Cookies[key];
        }

        /// <summary>
        /// 获取安全跳转Url
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="url">待跳转的url</param>
        /// <param name="vDefaultUrl">默认url(如url不合法,则返回默认url)</param>
        /// <param name="allowUrls">允许的url列表(netcore中,如果为null,则从WebSiteOptions配置中获取)</param>
        /// <returns></returns>
        public static string GetRedirectUrl(this HttpContext HttpContext, string url, string vDefaultUrl = null, params string[] allowUrls)
        {
            //获取默认地址
            string defaultUrl = "/";
            if (!string.IsNullOrEmpty(vDefaultUrl))
            {
                defaultUrl = vDefaultUrl;
            }

            if (string.IsNullOrEmpty(url))
                return defaultUrl;

            if (IsRelativeHost(url))
            {
                //本地地址，直接跳转
                return url;
            }
            else
            {
                //判断是否为允许的地址
                if (!Uri.TryCreate(url, UriKind.Absolute, out Uri redirectUri))
                {
                    //_logger.LogDebug($"请求地址:{url} 格式错误");
                    return defaultUrl;
                }

                if (allowUrls != null && allowUrls.Length > 0)
                {
                    foreach (var item in allowUrls)
                    {
                        if (string.Equals(redirectUri.Host, item, StringComparison.OrdinalIgnoreCase)
                            || (!string.IsNullOrEmpty(item)
                                && item.Length > 2
                                && item.StartsWith("*.")
                                && redirectUri.Host.Length >= item.Length - 2
                                && item.Substring(2, item.Length - 2).Equals(redirectUri.Host.Substring(redirectUri.Host.Length - (item.Length - 2)), StringComparison.OrdinalIgnoreCase)))
                        {
                            return url;
                        }
                    }
                }

                return defaultUrl;
            }
        }

        /// <summary>
        /// 是否本地址
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsRelativeHost(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            if (path.Length == 1)
            {
                return path[0] == '/';
            }
            return path[0] == '/' && path[1] != '/' && path[1] != '\\';
        }
    }
}
