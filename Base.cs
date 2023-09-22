using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor
{
    public static class Base
    {
        public static Type[] ValueTypes => new Type[] { typeof(string), typeof(DateTime) };
    }
}
