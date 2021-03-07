using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LabCenter.Shared.Extensions
{
    public static class JsonExtensions
    {
        public static async Task<T?> ReadFromJsonAsyncWithExceptionHandled<T>(this HttpContent content)
        {
            try
            {
                var result = await content.ReadFromJsonAsync<T>();
                return result;
            }
            catch
            {
                return default;
            }
        }
    }
}
