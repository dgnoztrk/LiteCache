using LiteCache.Client.Abstract;
using LiteCache.LiteCacheHelper.Abstract;

namespace LiteCache.LiteCacheHelper.Concrete
{
    public class LiteCacheService : ILiteCacheService
    {
        private readonly ILiteCacheClient _client;

        public LiteCacheService(ILiteCacheClient client)
        {
            _client = client;
            _client.ConnectAsync().Wait();
        }

        public async Task<string> GetAsync(string key)
        {
            return await _client.GetAsync(key);
        }

        public async Task SetAsync(string key, string value, TimeSpan exp)
        {
            await _client.SetAsync(key, value, exp);
        }

        public async Task DeleteAsync(string key)
        {
            await _client.DeleteAsync(key);
        }

        public async Task<List<string>> GetKeysAsync()
        {
            return await _client.GetKeysAsync();
        }
    }
}
