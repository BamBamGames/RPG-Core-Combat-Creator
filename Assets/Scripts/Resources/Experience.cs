using UnityEngine;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float experiencePoint = 0f;

        public void GainExperience(float experience)
        {
            experiencePoint += experience;
        }
    }
}