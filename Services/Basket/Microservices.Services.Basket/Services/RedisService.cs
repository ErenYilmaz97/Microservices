using StackExchange.Redis;

namespace Microservices.Services.Basket.Services
{
    public class RedisService
    {
        private readonly string _host;
        private readonly int _port;

        private IConnectionMultiplexer _connectionMultiplexer; 

        public RedisService(string host, int port)
        {
            _host = host;
            _port = port;
        }


        public void ConnectToRedisServer() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        public IDatabase GetRedisDatabase(int db = 1) => _connectionMultiplexer.GetDatabase(db);
    }
}