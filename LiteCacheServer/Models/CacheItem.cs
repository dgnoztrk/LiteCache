using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCacheServer.Models
{
    public class CacheItem
    {
        public string Value { get; }
        public TimeSpan Expiration { get; }

        public CacheItem(string value, TimeSpan expiration)
        {
            Value = value;
            Expiration = expiration;
        }

        public DateTime ExpirationDateTime => DateTime.UtcNow + Expiration;
    }
}
