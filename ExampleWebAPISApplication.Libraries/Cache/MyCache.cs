using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ExampleWebAPISApplication.Libraries.Cache
{
    public class MyCache : IMyCache
    {
        static string connectionString = string.Empty;
        private ConnectionMultiplexer _connection;

        private ConnectionMultiplexer Connection
        {
            get 
            {
                if (_connection == null)
                {
                    _connection = CreateMultiplexer();
                }
                return _connection;
            }
        }

        public static void InitializeConnectionString(string cnxString)
        {
            if (string.IsNullOrWhiteSpace(cnxString))
                throw new ArgumentNullException(nameof(cnxString));

            connectionString = cnxString;
        }

        private static ConnectionMultiplexer CreateMultiplexer()
        {
            return ConnectionMultiplexer.Connect(connectionString);
        }

        public MyCache()
        {

        } 

        public async Task<bool> SetString(string key, string value)
        {
            var cache = Connection.GetDatabase();
            return await cache.StringSetAsync(key, value);
        }
    }
}
