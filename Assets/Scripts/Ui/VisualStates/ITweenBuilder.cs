using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Drift.Ui
{
    public interface ITweenBuilder
    {
        UniTask Build(GameObject gameObject);
    }
}