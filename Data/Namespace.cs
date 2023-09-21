using MinecraftDatapackEditor.Data.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data
{
    public class Namespace
    {
        public string Name { get; set; }
        public Dimension[] Dimensions { get; set; }

        public Namespace(string name)
        {
            Name = name;
        }
    }
}
