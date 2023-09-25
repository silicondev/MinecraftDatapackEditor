using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftDatapackEditor.Interfaces;
using Newtonsoft.Json;

namespace MinecraftDatapackEditor.Data.Dimensions.Generation
{
    public class Generator : IRenderable
    {
        public string type { get; set; }
        public string settings { get; set; }
        public BiomeSource biome_source { get; set; }

        [JsonIgnore]
        public string Title => "generator";
        [JsonIgnore]
        public Type OriginType => typeof(Generator);
    }
}
