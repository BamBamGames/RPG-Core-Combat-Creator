using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
            }
            else
            {
                var health = fighter.GetTarget();
                GetComponent<Text>().text = string.Format("{0}/{1}", health.HealthPoints, health.MaxHealthPoints);
            }
        }
    }
}