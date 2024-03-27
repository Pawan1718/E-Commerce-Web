using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SushmaElectrical.Extensions
{
    public static class SessionExtensions
    {
        public static T GetObject<T>(this ISession session, string key)
        {
            byte[] data;
            if (session.TryGetValue(key, out data))
            {
                var jsonString = Encoding.UTF8.GetString(data);
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            return default;
        }

        public static void SetObject<T>(this ISession session, string key, T value)
        {
            var jsonString = JsonSerializer.Serialize(value);
            var data = Encoding.UTF8.GetBytes(jsonString);
            session.Set(key, data);
        }
    }
}
