using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Converters
{
    public class SingleArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(T) || objectType == typeof(T[]);

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jt = JToken.Load(reader);

            if (jt.Type == JTokenType.Array)
                return jt.Select(x => x.ToObject<T>()).ToArray();
            else
                return new T[] { jt.ToObject<T>() };

        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
