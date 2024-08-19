using LiteCacheServer.LiteCache.Abstract;
using LiteCacheServer.LiteCache.Concrete;
using LiteCacheServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json", optional: false, reloadOnChange: true)
    .Build();


var serviceProvider = new ServiceCollection()
    .Configure<LiteCacheConfig>(configuration.GetSection("LiteCacheConfig"))
    .AddSingleton<ILiteCache, LiteCacheService>()
    .BuildServiceProvider();

var liteCache = serviceProvider.GetService<ILiteCache>();


await liteCache.StartAsync();