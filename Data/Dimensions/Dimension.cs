using MinecraftDatapackEditor.Data.Dimensions.Generation;
using MinecraftDatapackEditor.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data.Dimensions
{
    public class Dimension : IRenderable
    {
        [JsonIgnore]
        public string Name { get; set; }
        public string type { get; set; }
        public Generator generator { get; set; }
        [JsonIgnore]
        public string Title => Name;
        [JsonIgnore]
        public Type OriginType => typeof(Dimension);
    }
}
