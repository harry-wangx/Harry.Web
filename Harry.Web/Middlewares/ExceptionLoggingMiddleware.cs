using Harry.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Harry.Web.Middlewares
{
    /// <summary>
    /// 错误日志记录中间件
    /// </summary>
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private static readonly string ajaxContentType = new MediaTypeHeaderValue("application/json")
        {
            Encoding = Encoding.UTF8
        }.ToString();


        public ExceptionLoggingMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory.CreateLogger<ExceptionLoggingMiddleware>();
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //记录日志
                _logger.LogError(ex, ex.Message);

                try
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 500;

                    if (context.Request.IsAjaxRequest())
                    {
                        //ajax调用
                        context.Response.ContentType = ajaxContentType;
                        await context.Response.WriteAsync("{'code':-1,'msg':'系统错误,请联系管理员.'}");
                    }
                    else
                    {
                        context.Response.ContentType = "text/html; charset=utf-8";
                        await context.Response.WriteAsync("系统错误,请联系管理员.", Encoding.UTF8);
                    }

                    return;
                }
                catch (Exception ex2)
                {
                    // 如果生成错误页时出现异常，重新抛出原始异常。
                    _logger.LogError(ex2, ex2.Message);
                }
                throw;
            }
        }
    }
}
