using MinecraftDatapackEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor
{
    public class DistinctedList<T, TKey> : RenderableList<T> where T : IRenderDistinctable<string>
    {
        public IList<T> Origin { get; }
        public Func<T, TKey> KeySelector { get; }
        public override Type? OriginType =>
            Origin.FirstOrDefault()?.OriginType;

        public DistinctedList(string title, IList<T> originalList, Func<T, TKey> keySelector) : base(title)
        {
            KeySelector = keySelector;
            Origin = originalList;
        }

        public DistinctedList(string title, IList<T> originalList, Func<T, TKey> keySelector, List<T> value) : base(title, value)
        {
            KeySelector = keySelector;
            Origin = originalList;
        }
    }
}
