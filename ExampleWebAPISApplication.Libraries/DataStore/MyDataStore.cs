using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebAPISApplication.Libraries.DataStore
{
    public class MyDataStore : IMyDataStore
    {
        private readonly string cosmosEndpoint;
        private readonly string cosmosKey;
        private static Database _cosmosDatabase;

        public MyDataStore(string cosmosEndpoint, string cosmosKey)
        {
            this.cosmosEndpoint = cosmosEndpoint;
            this.cosmosKey = cosmosKey;
        }

        private Database CosmosDatabase
        { 
            get 
            {
                if (_cosmosDatabase == null)
                {
                    CosmosClient client = new CosmosClient(cosmosEndpoint, cosmosKey);
                    _cosmosDatabase = client.GetDatabase("weather");
                }

                return _cosmosDatabase;
            } 
        }

        public async Task<dynamic> GetCurrentWeatherAsync()
        {
            Container container = CosmosDatabase.GetContainer("currentWeather");

            var item = await container.ReadItemAsync<dynamic>("3232423", new PartitionKey("3232423"));

            return item;
        }
    }
}
