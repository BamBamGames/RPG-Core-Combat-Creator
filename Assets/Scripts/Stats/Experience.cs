using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoint = 0f;

        // public delegate void ExperienceGainDelegate();
        public event Action onExperienceGained;

        public float ExperiencePoint { get => experiencePoint; }

        public void GainExperience(float experience)
        {
            experiencePoint += experience;
            onExperienceGained();
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