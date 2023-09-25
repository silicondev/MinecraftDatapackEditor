using MinecraftDatapackEditor.Converters;
using MinecraftDatapackEditor.Interfaces;
using MinecraftDatapackEditor.Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data.Dimensions.Generation
{
    public class BiomeParameters : IRenderable
    {
        [JsonConverter(typeof(Vector2DConverter))]
        public Vector2D temperature { get; set; }
        [JsonConverter(typeof(Vector2DConverter))]
        public Vector2D humidity { get; set; }
        [JsonConverter(typeof(Vector2DConverter))]
        public Vector2D continentalness { get; set; }
        [JsonConverter(typeof(Vector2DConverter))]
        public Vector2D weirdness { get; set; }
        [JsonConverter(typeof(Vector2DConverter))]
        public Vector2D depth { get; set; }
        [JsonConverter(typeof(Vector2DConverter))]
        public Vector2D offset { get; set; }

        [JsonIgnore]
        public string Title => "parameters";
        [JsonIgnore]
        public Type OriginType => typeof(BiomeParameters);
    }
}
