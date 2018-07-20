using Harry.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using System;

namespace Harry.Web
{
    /// <summary>
    /// 仅允许通过Ajax调用
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public AjaxOnlyAttribute(bool ignore = false)
        {
            Ignore = ignore;
        }

        /// <summary>
        /// 是否忽略检查
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 验证请求
        /// </summary>
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            if (Ignore)
                return true;
            return routeContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}
