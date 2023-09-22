using MinecraftDatapackEditor.Attributes;
using MinecraftDatapackEditor.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace MinecraftDatapackEditor
{
    public static class Extensions
    {
        public static void Render(this TreeViewItem parent, IRenderable obj) =>
            parent._Render(obj, obj.Title);

        public static void Render(this TreeView parent, IRenderable obj) =>
            parent._Render(obj, obj.Title);

        public static void Render(this TreeViewItem parent, object obj, string title) =>
            parent._Render(obj, title);

        public static void Render(this TreeView parent, object obj, string title) =>
            parent._Render(obj, title);

        private static void _Render(this ItemsControl parent, object obj, string title)
        {
            if (obj == null)
                return;

            var initialNode = new TreeViewItem()
            {
                Header = title,
                Tag = obj
            };

            var type = obj.GetType();

            if (type.IsEnumerable() && !Base.ValueTypes.Contains(type))
            {
                var elType = type.GetArrayType();

                var arr = obj.GetArray();

                if (arr == null)
                    return;

                if (elType.CheckInterface<IRenderDistinctable<string>>())
                    arr = arr.Cast<IRenderDistinctable<string>>().ToArray().GetDistinct<string, IRenderDistinctable<string>>().Select(x => new RenderableList<object>(x.Title, ((List<IRenderDistinctable<string>>)x).Cast<object>().ToList())).ToArray();

                initialNode.Header += $"[{arr.Length}]";

                var isRenderable = elType.IsRenderableType();

                for (int i = 0; i < arr.Length; i++)
                    if (arr.GetValue(i) != null)
                        if (isRenderable)
                        {
                            var renderable = (IRenderable)arr.GetValue(i);
                            initialNode.Render(arr.GetValue(i), $"[{i}] {renderable.Title}");
                        }
                        else
                            initialNode.Render(arr.GetValue(i), $"[{i}]");
            }
            // Is a value type that should be displayed as is
            else if (type.IsReadableType())
                initialNode.Header += $" ({type.Name})";
            // Is an instance of IRenderable, is an instance of a class and has a title to give
            else if (type.IsRenderableType())
            {
                var props = type.GetProperties();

                // Loop through all properties
                foreach (var prop in props)
                {
                    var propType = prop.PropertyType;
                    object? value = prop.GetValue(obj);

                    // Ignore those with [JsonIgnore]
                    if (prop.CheckAttribute<JsonIgnoreAttribute>())
                        continue;

                    // Is a value type that should be displayed as is
                    if (value != null)
                        initialNode.Render(value, prop.Name);
                }
            }

            parent.Items.Add(initialNode);
        }

        public static bool CheckAttribute<T>(this MemberInfo mi) where T : Attribute => Attribute.GetCustomAttribute(mi, typeof(T)) != null;
        public static bool CheckInterface<T>(this Type mi) => mi.GetInterfaces().Contains(typeof(T));

        public static bool IsReadableType(this Type t) => t.IsValueType || Base.ValueTypes.Contains(t);
        public static bool IsRenderableType(this Type t) => t.IsArray ? t.GetElementType().CheckInterface<IRenderable>() : t.CheckInterface<IRenderable>();
        public static bool CanRenderOrRead(this Type t) => t.IsRenderableType() || t.IsReadableType();
        public static bool IsEnumerable(this Type t) => t.IsArray || t.CheckInterface<ICollection>();
        public static Type? GetArrayType(this Type t)
        {
            if (!t.IsEnumerable())
                return null;

            var elemType = t.GetElementType();

            if (elemType != null)
                return elemType;

            var addMethod = t.GetMethod("Add");

            if (addMethod == null)
                return null;

            var pa = addMethod.GetParameters();

            if (pa.Length != 1)
                return null;

            return pa[0].ParameterType;
        }

        public static List<RenderableList<TValue>> GetDistinct<TKey, TValue>(this IRenderDistinctable<TKey>[] list) where TValue : IRenderDistinctable<TKey>
        {
            var dict = new Dictionary<TKey, RenderableList<TValue>>();

            foreach (TValue item in list)
            {
                if (item == null)
                    continue;

                TKey key = item.GetDistinct();

                if (key == null)
                    continue;

                if (dict.ContainsKey(key))
                    dict[key].Add(item);
                else
                    dict.Add(key, new RenderableList<TValue>(item.Title) { item });
            }

            return dict.Values.ToList();
        }

        public static object[]? GetArray(this object obj)
        {
            var type = obj.GetType();

            if (!type.IsEnumerable())
                return null;

            if (type.IsArray)
                return ((Array)obj).Cast<object>().ToArray();

            var coll = (ICollection)obj;
            var arr = new object[coll.Count];
            coll.CopyTo(arr, 0);
            return arr;
        }
    }
}
