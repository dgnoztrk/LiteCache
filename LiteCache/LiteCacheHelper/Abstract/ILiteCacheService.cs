namespace LiteCache.LiteCacheHelper.Abstract
{
    public interface ILiteCacheService
    {
        Task<string> GetAsync(string key);
        Task<List<string>> GetKeysAsync();
        Task SetAsync(string key, string value, TimeSpan exp);
        Task DeleteAsync(string key);
    }
}
