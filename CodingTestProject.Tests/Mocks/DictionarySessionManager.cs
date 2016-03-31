using CodingTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTestProject.Tests
{
    public class DictionarySessionManager : ISessionManager
    {
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public void Add(string key, string value)
        {
            dict.Add(key, value);
        }

        public string Get(string key)
        {
            return dict.ContainsKey(key) ? dict[key] : string.Empty;
        }

        public bool HasKey(string key)
        {
            return dict.ContainsKey(key);
        }

        public void Remove(string key)
        {
            dict.Remove(key);
        }
    }
}
