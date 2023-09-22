using MinecraftDatapackEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DistinctableAttribute : Attribute
    {

        public DistinctableAttribute() { }

        public bool Enable { get; set; } = true;

        public List<RenderableList<T>> GetDistinct<T, TKey>(T[] value) where T : IDistinctable<TKey>
        {
            var dict = new Dictionary<TKey, List<T>>();

            foreach (T item in value)
            {
                if (item == null)
                    continue;

                TKey key = item.GetDistinct();

                if (key == null)
                    continue;

                if (dict.ContainsKey(key))
                    dict[key].Add(item);
                else
                    dict.Add(key, new List<T>() { item });
            }

            return dict.Select(x => new RenderableList<T>(x.Key.ToString(), x.Value)).ToList();
        }
    }
}
