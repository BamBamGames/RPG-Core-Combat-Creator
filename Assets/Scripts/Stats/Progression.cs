using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (var item in characterClasses)
            {
                if (item.characterClass == characterClass)
                {
                    
                    // return item.health[level - 1];
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