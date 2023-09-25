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

        public string Data => ToString();

        public override string ToString()
        {
            var dbs = new List<double>();
            if (X != null)
                dbs.Add(X.Value);
            if (Y != null)
                dbs.Add(Y.Value);
            return string.Join(", ", dbs.ToArray());
        }
    }
}
