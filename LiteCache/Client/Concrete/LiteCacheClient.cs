using LiteCache.Client.Abstract;
using LiteCache.Models;
using Microsoft.Extensions.Options;
using System.Net.Sockets;
using System.Text;

namespace LiteCache.Client.Concrete
{
    public class LiteCacheClient : ILiteCacheClient
    {
        private readonly string _ip;
        private readonly int _port;
        private readonly string _password;
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;

        public LiteCacheClient(IOptions<LiteCacheConfig> conf)
        {
            _ip = conf.Value.IP;
            _port = conf.Value.Port;
            _password = conf.Value.Password;
        }

        public async Task ConnectAsync()
        {
            _client = new TcpClient();
            await _client.ConnectAsync(_ip, _port);

            var networkStream = _client.GetStream();
            _reader = new StreamReader(networkStream, Encoding.UTF8);
            _writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            await AuthenticateAsync();
        }

        private async Task AuthenticateAsync()
        {
            if (!string.IsNullOrEmpty(_password))
            {
                await _writer.WriteLineAsync(_password);
                var response = await _reader.ReadLineAsync();
                if (response != "Authentication successful.")
                {
                    throw new Exception("Authentication failed.");
                }
            }
        }

        public async Task<string> GetAsync(string key)
        {
            await _writer.WriteLineAsync($"GET {key}");
            return await _reader.ReadLineAsync();
        }

        public async Task SetAsync(string key, string value, TimeSpan exp)
        {
            await _writer.WriteLineAsync($"SET {key} {value} {exp}");
            await _reader.ReadLineAsync(); // Read response
        }

        public async Task DeleteAsync(string key)
        {
            await _writer.WriteLineAsync($"DELETE {key}");
            await _reader.ReadLineAsync(); // Read response
        }

        public void Dispose()
        {
            _writer?.Dispose();
            _reader?.Dispose();
            _client?.Close();
        }

        public async Task<List<string>> GetKeysAsync()
        {
            await _writer.WriteLineAsync($"GETKEYS");
            var result = await _reader.ReadLineAsync();
            if (result != "(nil)")
            {
                return result.Split(", ").ToList();
            }
            return new List<string>();
        }
    }
}
