using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach (var progression in characterClasses)
            {
                if (progression.characterClass != characterClass) continue;

                foreach (var progressionStat in progression.stats)
                {
                    if (progressionStat.stat != stat) continue;
                    if (progressionStat.levels.Length < level) continue;

                    return progressionStat.levels[level - 1];
                }
            }

            return 0;
        }

        /**
         * if we don't have [System.Serializable]
         * Assets\Scripts\Stats\Progression.cs(13,17): warning CS0414: The field 'Progression.ProgressionCharacterClass.value' is assigned but its value is never used
         * this class here is not marked as something that unity knows how to serialize
         * use attribute [System.Serializable]
         */
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;

        }
    }
}