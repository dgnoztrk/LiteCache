# LiteCache

> **LiteCache** is a lightweight, high-performance, and in-memory key-value store designed as a simple alternative to Redis. Built for developers who need fast and efficient caching without the overhead, LiteCache offers the essential features you need in a blazing-fast package.

## 🚀 Why LiteCache?

In a world dominated by powerful tools like Redis, sometimes you just need something simpler, something more focused. **LiteCache** was built with one goal in mind: to provide a minimalistic, easy-to-use caching solution that doesn't compromise on performance.

- **Ultra-Fast**: Optimized for speed, LiteCache delivers exceptional performance in handling your cache needs.
- **Minimal Dependencies**: No external dependencies or complex setup. Just drop it in and go.
- **Thread-Safe**: Built with concurrency in mind, ensuring your data is safe even under heavy load.

## 🌟 Features

- **Lightweight and Fast**: Minimalistic design ensures maximum speed.
- **Simple to Use**: Focuses on the essential features you need.
- **Customizable**: Easily configure and extend LiteCache to suit your needs.

### Internal Data Management

- **ConcurrentDictionary**:
   - **A thread-safe collection used internally to store cache items.**
   - **Ensures high performance and safety in multi-threaded environments.**
   - **Automatically handles synchronization, making it ideal for concurrent access scenarios.**

## 🔧 Getting Started

Since LiteCache isn't available on NuGet yet, you'll need to clone this repository and add the project to your solution manually.

## 📚 API Reference

### LiteCacheClient

1. `Task ConnectAsync();`
   - Establishes a connection to the LiteCache server.
2. `Task<string> GetAsync(string key);`
   - Retrieves the value associated with the specified key.
3. `Task SetAsync(string key, string value, int expirationInSeconds);`
   - Stores a value with a specified expiration time.
4. `Task DeleteAsync(string key);`
   - Deletes the specified key from the cache.

### Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/dgnoztrk/LiteCache.git
    ```

2. Add the `LiteCache` project to your solution in Visual Studio or your preferred IDE.

3. Reference the `LiteCache` project in your application.

### Basic Configuration

To get started with LiteCache, you can configure it with just an IP address, port, and (optional) password:


```csharp

builder.Services.AddLiteCache(builder.Configuration.GetSection("LiteCacheConfig").Get<LiteCacheConfig>());


var client = new LiteCacheClient(options);
await client.ConnectAsync();
```
### LiteCacheConfig

1. `string IP;`
   - The IP address of the LiteCache server.
2. `int Port;`
   - The port where LiteCache is running.
3. `string Password;`
   - The password for authentication.

Edit in appsettings.json
```json
{
  "LiteCacheConfig": {
    "IP": "127.0.0.1",
    "Password": "password",
    "Port": 6379
  }
}
```
DI and for example;
```csharp
private readonly ILiteCacheService _liteCache;
public Controller(ILiteCacheService liteCache)
{
 _liteCache = liteCache;
}

[HttpPost("set-val")]
public async Task<IActionResult> SetValue(string key, string value)
{
    await _liteCache.SetAsync(key, value, TimeSpan.FromHours(1));
    return Ok();
}

[HttpGet("get-val")]
public async Task<IActionResult> GetValue(string key)
{
    var value = await _liteCache.GetAsync(key);
    return Ok(value);
}

[HttpDelete("del-val")]
public async Task<IActionResult> DeleteValue(string key)
{
    await _liteCache.DeleteAsync(key);
    return Ok();
}

[HttpGet("get-all-key")]
public async Task<IActionResult> GetAllKey()
{
    var keys = await _liteCache.GetKeysAsync();
    return Ok(keys);
}
```

Happy Code !
