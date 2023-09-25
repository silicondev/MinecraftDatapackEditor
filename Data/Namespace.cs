using MinecraftDatapackEditor.Data.Dimensions;
using MinecraftDatapackEditor.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data
{
    public class Namespace : IRenderable
    {
        public string Name { get; set; }
        public Dimension[] Dimensions { get; set; }

        [JsonIgnore]
        public string Title => Name;
        [JsonIgnore]
        public Type OriginType => typeof(Namespace);

        public Namespace(string name)
        {
            Name = name;
        }
    }
}
