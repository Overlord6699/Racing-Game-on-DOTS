using Unity.Entities;

namespace Drift
{
    public interface ISurfaceService
    {
        SurfaceDefinition[] Surfaces { get; }
        
        Entity GetSkidmarksPrefab(int index);
        
        Entity GetSurfaceSoundPrefab(int index);

        Entity GetCollisionSoundPrefab(int index);
    }
}