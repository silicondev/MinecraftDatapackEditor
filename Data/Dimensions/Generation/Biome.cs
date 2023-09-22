using MinecraftDatapackEditor.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MinecraftDatapackEditor.Data.Dimensions.Generation
{
    public class Biome : IRenderDistinctable<string>
    {
        [JsonProperty("biome")]
        public string Id { get; set; }
        [JsonProperty("parameters")]
        public BiomeParameters Parameters { get; set; }

        [JsonIgnore]
        public string Title => Id;

        public string GetDistinct() => Id;
    }
}
