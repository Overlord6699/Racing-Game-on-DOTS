using UnityEngine;

namespace Drift
{
    [CreateAssetMenu(menuName = "Database/Surface")]
    public class SurfaceDefinition : ScriptableObject
    {
        public int RuntimeIndex;
        public PrefabEntity Skidmarks;
        public PrefabEntity SurfaceSound;
        
        [Header("Collisions")]
        public PrefabEntity CollisionSound;
        public Vector2 ImpulseToVolumeRemap = new Vector2(50, 500);
        
        [Header("Particles")] 
        public PrefabEntity Particles;
    }
}