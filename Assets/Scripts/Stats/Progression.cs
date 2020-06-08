using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            var levels = lookupTable[characterClass][stat];
            if (levels.Length >= level)
            {
                return levels[level - 1];
            }

            return 0;
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            return lookupTable[characterClass][stat].Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (var progression in characterClasses)
            {
                var lookupItem = new Dictionary<Stat, float[]>();
                foreach (var progressionStat in progression.stats)
                {
                    lookupItem[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progression.characterClass] = lookupItem;
            }
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