using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data.Dimensions.Generation
{
    public class Biome
    {
        [JsonProperty("biome")]
        public string Id { get; set; }
        [JsonProperty("parameters")]
        public BiomeParameters Parameters { get; set; }
    }
}
