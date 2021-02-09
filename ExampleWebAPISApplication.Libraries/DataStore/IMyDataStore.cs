using System.Threading.Tasks;

namespace ExampleWebAPISApplication.Libraries.DataStore
{
    public interface IMyDataStore
    {
        Task<dynamic> GetCurrentWeatherAsync();
    }
}
