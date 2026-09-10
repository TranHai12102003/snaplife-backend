using System.Text.Json;

namespace SL.Domain.Common.Ultilities
{
    public static class JsonHelper
    {
        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
