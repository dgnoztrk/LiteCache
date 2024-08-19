namespace LiteCacheServer.LiteCache.Abstract
{
    public interface ILiteCache
    {
        /// <summary>
        /// Servisi başlatır ve TCP bağlantılarını kabul eder.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Key-value çiftini cache'e ekler veya günceller.
        /// </summary>
        /// <param name="key">Cache'te saklanacak anahtar.</param>
        /// <param name="value">Cache'te saklanacak değer.</param>
        /// <param name="exp">Verinin geçerlilik süresi (Timespan cinsinden).</param>
        Task SetAsync(string key, string value, TimeSpan exp);

        /// <summary>
        /// Cache'den bir key'e karşılık gelen değeri getirir.
        /// </summary>
        /// <param name="key">Değeri getirilecek anahtar.</param>
        /// <returns>Cache'deki değer ya da (nil) eğer değer mevcut değilse.</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// Cache'den bir key'e karşılık gelen değeri siler.
        /// </summary>
        /// <param name="key">Silinecek anahtar.</param>
        Task DeleteAsync(string key);
    }
}