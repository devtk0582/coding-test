using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingTestProject.Models
{
    public interface ISessionManager
    {
        void Add(string key, string value);
        string Get(string key);
        bool HasKey(string key);
        void Remove(string key);
    }
}