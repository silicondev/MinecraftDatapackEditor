using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Interfaces
{
    public interface IDistinctable<T>
    {
        public T GetDistinct();
    }
}
