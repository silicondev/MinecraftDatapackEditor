using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftDatapackEditor.Structs;

namespace MinecraftDatapackEditor.Converters
{
    public class Vector2DConverter : SingleArrayConverter<double>
    {
        public override bool CanConvert(Type objectType) =>
            base.CanConvert(objectType);

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var valsObj = base.ReadJson(reader, objectType, existingValue, serializer);
            if (valsObj == null)
                return null;

            var vals = (double[])valsObj;

            if (vals.Length == 1)
                return new Vector2D()
                {
                    X = vals[0]
                };
            else if (vals.Length == 2)
                return new Vector2D()
                {
                    X = vals[0],
                    Y = vals[1]
                };

            return null;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
                return;

            var vec = (Vector2D)value;
            var result = vec.X != null ? vec.Y != null ? new double[] { vec.X.Value, vec.Y.Value } : new double[] { vec.X.Value } : new double[0];
            base.WriteJson(writer, result, serializer);
        }
    }
}
