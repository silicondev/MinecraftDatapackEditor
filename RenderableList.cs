using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;
using MinecraftDatapackEditor.Interfaces;

namespace MinecraftDatapackEditor
{
    public class RenderableList<T> : List<T>, IRenderable
    {
        public string Title { get; }
        public RenderableList(string title) : base()
        {
            Title = title;
        }

        public RenderableList(string title, List<T> value) : base(value)
        {
            Title = title;
        }

        public RenderableList<TOut> Cast<TOut>()
        {
            var list = new List<T>();
            list.AddRange(ToArray());
            var c = list.Cast<TOut>().ToList();
            return new RenderableList<TOut>(Title, c);
        }
    }
}
