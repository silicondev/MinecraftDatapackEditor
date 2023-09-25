using MinecraftDatapackEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Structs
{
    public struct Vector2D : IRenderDivert
    {
        public double? X { get; set; }
        public double? Y { get; set; }
        public string Data => $"{X}, {Y};";
    }
}
