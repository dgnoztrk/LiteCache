using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using System.Text;
using LiteCacheServer.Models;
using LiteCacheServer.LiteCache.Abstract;
using Microsoft.Extensions.Options;

namespace LiteCacheServer.LiteCache.Concrete
{
    public class LiteCacheService : ILiteCache
    {
        private readonly TcpListener _listener;
        private readonly ConcurrentDictionary<string, CacheItem> _dataStore = new();
        private readonly LiteCacheConfig _serveroptions;
        public LiteCacheService(IOptions<LiteCacheConfig> serveroptions)
        {
            _serveroptions = serveroptions.Value;
            _listener = new TcpListener(IPAddress.Parse(_serveroptions.IP), _serveroptions.Port);
            //_listener = new TcpListener(IPAddress.Any, _serveroptions.Port);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine($"Listening on {_listener.LocalEndpoint}...");

            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using var networkStream = client.GetStream();
            using var reader = new StreamReader(networkStream, Encoding.UTF8);
            using var writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            // Authentication Step
            if (!await AuthenticateAsync(reader, writer))
            {
                return;
            }

            while (client.Connected)
            {
                string command = await reader.ReadLineAsync();
                if (command != null)
                {
                    await ProcessCommandAsync(command, writer);
                }
            }
        }

        private async Task<bool> AuthenticateAsync(StreamReader reader, StreamWriter writer)
        {
            if (string.IsNullOrEmpty(_serveroptions.Password))
            {
                return true;
            }

            //await writer.WriteLineAsync("Enter password:");
            string inputPassword = await reader.ReadLineAsync();

            if (inputPassword == _serveroptions.Password)
            {
                await writer.WriteLineAsync("Authentication successful.");
                return true;
            }

            await writer.WriteLineAsync("Authentication failed.");
            return false;
        }

        private async Task ProcessCommandAsync(string command, StreamWriter writer)
        {
            var parts = command.Split(' ');
            string operation = parts[0].ToUpper();

            switch (operation)
            {
                case "SET":
                    await HandleSetCommandAsync(parts, writer);
                    break;
                case "GET":
                    await HandleGetCommandAsync(parts, writer);
                    break;
                case "GETKEYS":
                    await HandleGetAllCommandAsync(parts, writer);
                    break;
                case "DELETE":
                    await HandleDeleteCommandAsync(parts, writer);
                    break;
                default:
                    await writer.WriteLineAsync("Invalid command.");
                    break;
            }
        }

        private async Task HandleSetCommandAsync(string[] parts, StreamWriter writer)
        {
            if (parts.Length < 4)
            {
                await writer.WriteLineAsync("Usage: SET key value expiration Timespan");
                return;
            }

            string key = parts[1];
            string value = parts[2];
            if (!TimeSpan.TryParse(parts[3], out TimeSpan expiration))
            {
                await writer.WriteLineAsync("Expiration must be an TimeSpan.");
                return;
            }

            var cacheItem = new CacheItem(value, expiration);
            _dataStore[key] = cacheItem;

            await writer.WriteLineAsync("OK");
        }

        private async Task HandleGetCommandAsync(string[] parts, StreamWriter writer)
        {
            if (parts.Length < 2)
            {
                await writer.WriteLineAsync("Usage: GET key");
                return;
            }

            string key = parts[1];
            if (_dataStore.TryGetValue(key, out CacheItem item) && item.ExpirationDateTime > DateTime.UtcNow)
            {
                await writer.WriteLineAsync(item.Value);
            }
            else
            {
                await writer.WriteLineAsync("(nil)");
            }
        }

        private async Task HandleGetAllCommandAsync(string[] parts, StreamWriter writer)
        {
            var keys = _dataStore.Keys.ToList();
            if (keys.Count() > 0)
            {
                var str = string.Join(", ", keys);
                await writer.WriteLineAsync(str);
            }
            else
            {
                await writer.WriteLineAsync("(nil)");
            }
        }

        private async Task HandleDeleteCommandAsync(string[] parts, StreamWriter writer)
        {
            if (parts.Length < 2)
            {
                await writer.WriteLineAsync("Usage: DELETE key");
                return;
            }

            string key = parts[1];
            if (_dataStore.TryRemove(key, out _))
            {
                await writer.WriteLineAsync("OK");
            }
            else
            {
                await writer.WriteLineAsync("(nil)");
            }
        }

        public async Task SetAsync(string key, string value, TimeSpan exp)
        {
            var cacheItem = new CacheItem(value, exp);
            _dataStore[key] = cacheItem;
        }

        public async Task<string> GetAsync(string key)
        {
            if (_dataStore.TryGetValue(key, out CacheItem item) && item.ExpirationDateTime > DateTime.UtcNow)
            {
                return item.Value;
            }
            return "(nil)";
        }

        public async Task DeleteAsync(string key)
        {
            _dataStore.TryRemove(key, out _);
        }
    }

}
