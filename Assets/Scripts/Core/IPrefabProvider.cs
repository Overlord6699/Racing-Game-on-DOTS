using System.Collections.Generic;
using UnityEngine;

namespace Drift
{
    public interface IPrefabProvider
    {
        void DeclarePrefabs(List<GameObject> referencedPrefabs);
        
        void PreparePrefabs(GameObjectConversionSystem conversionSystem);
    }
}