using System;
using Newtonsoft.Json;

namespace InputLayer.Common.Serialization
{
    public static class Serializer
    {
        private static readonly Lazy<JsonSerializerSettings> _settings =
            new Lazy<JsonSerializerSettings>(() => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                SerializationBinder = new BackwardCompatibleSerializationBinder()
            });

        public static T Deserialize<T>(string content)
            => JsonConvert.DeserializeObject<T>(content, _settings.Value);
    }
}