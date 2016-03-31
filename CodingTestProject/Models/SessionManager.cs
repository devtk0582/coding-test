using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingTestProject.Models
{
    public class SessionManager : ISessionManager
    {
        public void Add(string key, string value)
        {
            HttpContext.Current.Session.Add(key, value);
        }

        public string Get(string key)
        {
            return HttpContext.Current.Session[key] != null ? HttpContext.Current.Session[key].ToString() : string.Empty;
        }

        public bool HasKey(string key)
        {
            return HttpContext.Current.Session[key] != null;
        }

        public void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
    }
}