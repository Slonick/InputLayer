using System;
using Newtonsoft.Json;

namespace InputLayer.Common.Utils
{
    public static class Serializer
    {
        private static readonly Lazy<JsonSerializerSettings> _settings =
            new Lazy<JsonSerializerSettings>(() => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

        public static T Deserialize<T>(string content) => JsonConvert.DeserializeObject<T>(content, _settings.Value);

        public static string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, _settings.Value);
    }
}