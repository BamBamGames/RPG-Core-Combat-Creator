using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        /**
         * if we don't have [System.Serializable]
         * Assets\Scripts\Stats\Progression.cs(13,17): warning CS0414: The field 'Progression.ProgressionCharacterClass.value' is assigned but its value is never used
         * this class here is not marked as something that unity knows how to serialize
         * use attribute [System.Serializable]
         */
        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] public CharacterClass characterClass;
            [SerializeField] public float[] health;
        }
    }
}