using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebAPISApplication.Libraries.Interfaces
{
    public interface IMyCache
    {
        Task<bool> SetString(string key, string value);
    }
}
