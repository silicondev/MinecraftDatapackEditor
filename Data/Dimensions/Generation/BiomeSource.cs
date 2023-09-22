using MinecraftDatapackEditor.Attributes;
using MinecraftDatapackEditor.Interfaces;
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

        public string Title => "biome_source";
    }
}
