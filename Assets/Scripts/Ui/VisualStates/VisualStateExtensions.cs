using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Drift.Ui
{
    internal static class VisualStateExtensions
    {
        public static IEnumerable<T> GetFirstDescendants<T>(this Transform transform)
        {
            var ls = ListPool<T>.Get();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                ls.Clear();
                child.GetComponents(ls);
                foreach (var l in ls)
                {
                    yield return l;
                }
                
                if (ls.Count != 0) continue;
                foreach (var firstDescendant in GetFirstDescendants<T>(child))
                {
                    yield return firstDescendant;
                }
            }

            ListPool<T>.Release(ls);
        }
    }
}