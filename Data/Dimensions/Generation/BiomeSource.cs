using MinecraftDatapackEditor.Attributes;
using MinecraftDatapackEditor.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data.Dimensions.Generation
{
    public class BiomeSource : IRenderable
    {
        public string type { get; set; }
        public Biome[] biomes { get; set; }

        [JsonIgnore]
        public string Title => "biome_source";
        [JsonIgnore]
        public Type? OriginType => typeof(BiomeSource);
    }
}
