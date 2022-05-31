using UnityEngine;

namespace Drift
{
    [CreateAssetMenu(menuName = "Database/Database")]
    public class Database : ScriptableObject, IDatabaseService
    {
        [SerializeField]
        private LevelDefinition[] levels;

        public LevelDefinition[] Levels => levels;
    }
}