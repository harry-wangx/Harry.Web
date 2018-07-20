using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Harry.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// 获取登录用户ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}