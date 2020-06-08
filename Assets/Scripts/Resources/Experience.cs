using RPG.Saving;
using UnityEngine;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoint = 0f;

        public float ExperiencePoint { get => experiencePoint; }

        public void GainExperience(float experience)
        {
            experiencePoint += experience;
        }

        public object CaptureState()
        {
            return experiencePoint;
        }

        public void RestoreState(object state)
        {
            experiencePoint = (float)state;
        }
    }
}