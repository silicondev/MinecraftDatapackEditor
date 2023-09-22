using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftDatapackEditor.Interfaces;

namespace MinecraftDatapackEditor.Data.Dimensions.Generation
{
    public class Generator : IRenderable
    {
        public string type { get; set; }
        public string settings { get; set; }
        public BiomeSource biome_source { get; set; }

        public string Title => "generator";
    }
}
