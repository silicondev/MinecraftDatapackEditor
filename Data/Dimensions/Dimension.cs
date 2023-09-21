using MinecraftDatapackEditor.Data.Dimensions.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data.Dimensions
{
    public class Dimension
    {
        public string type { get; set; }
        public Generator generator { get; set; }
    }
}
