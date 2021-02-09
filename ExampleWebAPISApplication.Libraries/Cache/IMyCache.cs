using System.Threading.Tasks;

namespace ExampleWebAPISApplication.Libraries.Cache
{
    public interface IMyCache
    {
        Task<bool> SetString(string key, string value);
    }
}
