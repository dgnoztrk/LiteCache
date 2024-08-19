namespace LiteCache.Client.Abstract
{
    public interface ILiteCacheClient
    {
        /// <summary>
        /// Bağlantıyı başlatır.
        /// </summary>
        Task ConnectAsync();

        /// <summary>
        /// Belirli bir anahtar için değeri alır.
        /// </summary>
        /// <param name="key">Anahtar</param>
        /// <returns>Değer</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// Cache içerisindeki tüm key'leri getirir.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetKeysAsync();

        /// <summary>
        /// Belirli bir anahtar için değeri ayarlar ve süresini belirler.
        /// </summary>
        /// <param name="key">Anahtar</param>
        /// <param name="value">Değer</param>
        /// <param name="exp">Süresi (saniye cinsinden)</param>
        Task SetAsync(string key, string value, TimeSpan exp);

        /// <summary>
        /// Belirli bir anahtarı siler.
        /// </summary>
        /// <param name="key">Anahtar</param>
        Task DeleteAsync(string key);

        /// <summary>
        /// Kaynakları serbest bırakır.
        /// </summary>
        void Dispose();
    }
}
