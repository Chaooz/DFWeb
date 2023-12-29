


using Microsoft.AspNetCore.Http;

namespace DarkFactorCoreNet.Provider
{
    public interface ICookieProvider
    {
        string GetCookie(string key);
        void SetCookie(string key, string value);
        void RemoveCookie(string key);
    }

    public class CookieProvider : ICookieProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CookieProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetCookie(string key)
        {
            var cookie = httpContextAccessor.HttpContext.Request.Cookies[key];
            if (cookie != null)
            {
                return cookie;
            }
            return "";
        }

        public void SetCookie(string key, string value)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Append(key, value);
        }

        public void RemoveCookie(string key)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }
    }
}