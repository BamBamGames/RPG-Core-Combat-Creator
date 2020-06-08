using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;

        int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
        }

        private void Update()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                print("Levelled Up !!!");
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentExp = experience.ExperiencePoint;
            var penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, CharacterClass.Player);
            for (int level = 1; level < penultimateLevel; level++)
            {
                var expToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, CharacterClass.Player, level);
                if (expToLevelUp > currentExp)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}