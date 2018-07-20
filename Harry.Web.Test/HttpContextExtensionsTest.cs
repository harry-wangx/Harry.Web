using Harry.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Harry.Web.Test
{
    public class HttpContextExtensionsTest
    {
        [Fact]
        public void GetRedirectUrl()
        {
            string[] allowedUrls = new[] {
                "a.abc.com",
                "*.bbc.com"
            };
            //本地地址,原样返回
            Assert.Equal("/abc/ddd", HttpContextExtensions.GetRedirectUrl(null, "/abc/ddd", "/", allowedUrls));
            //域名未授权
            Assert.Equal("/", HttpContextExtensions.GetRedirectUrl(null, "http://a.abcd.com/test", "/", allowedUrls));
            //以下个地址都允许返回
            Assert.Equal("http://a.abc.com/test", HttpContextExtensions.GetRedirectUrl(null, "http://a.abc.com/test", "/", allowedUrls));
            Assert.Equal("http://b.bbc.com/test", HttpContextExtensions.GetRedirectUrl(null, "http://b.bbc.com/test", "/", allowedUrls));
        }
    }
}
