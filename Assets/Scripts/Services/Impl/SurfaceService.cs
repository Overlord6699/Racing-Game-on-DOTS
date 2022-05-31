using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Drift
{
    [CreateAssetMenu(menuName = "Database/Surfaces")]
    public class SurfaceService : ScriptableObject, ISurfaceService, IPrefabProvider
    {
        [SerializeField]
        private SurfaceDefinition[] surfaces;
        
        public SurfaceDefinition[] Surfaces => surfaces;
        
        public void DeclarePrefabs(List<GameObject> referencedPrefabs)
        {
            for (var index = 0; index < surfaces.Length; index++)
            {
                var surfaceDefinition = surfaces[index];
                surfaceDefinition.RuntimeIndex = index;
                surfaceDefinition.Skidmarks.DeclarePrefab(referencedPrefabs);
                surfaceDefinition.SurfaceSound.DeclarePrefab(referencedPrefabs);
                surfaceDefinition.CollisionSound.DeclarePrefab(referencedPrefabs);
                surfaceDefinition.Particles.DeclarePrefab(referencedPrefabs);
            }
        }
        
        public void PreparePrefabs(GameObjectConversionSystem conversionSystem)
        {
            for (var index = 0; index < surfaces.Length; index++)
            {
                var surfaceDefinition = surfaces[index];
                surfaceDefinition.Skidmarks.PreparePrefab(conversionSystem);
                surfaceDefinition.SurfaceSound.PreparePrefab(conversionSystem);
                surfaceDefinition.CollisionSound.PreparePrefab(conversionSystem);
                surfaceDefinition.Particles.PreparePrefab(conversionSystem);
            }
        }
        
        public Entity GetSkidmarksPrefab(int surfaceIndex)
        {
            return surfaces[surfaceIndex].Skidmarks;
        }
        
        public Entity GetSurfaceSoundPrefab(int surfaceIndex)
        {
            return surfaces[surfaceIndex].SurfaceSound;
        }
        
        public Entity GetCollisionSoundPrefab(int surfaceIndex)
        {
            return surfaces[surfaceIndex].CollisionSound;
        }
    }
}