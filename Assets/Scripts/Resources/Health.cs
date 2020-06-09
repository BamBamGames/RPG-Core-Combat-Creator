using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [Tooltip("升级后血量的百分比, 设置为小于0关闭这个特性, 关闭特性后按照当前血量的百分比生成血量")]
        [SerializeField] private float regenationPercentage = -1f;

        private float maxHealthPoints = -1f;
        private float healthPoints = -1f;
        private bool isDead = false;

        public float MaxHealthPoints { get => maxHealthPoints; }
        public float HealthPoints { get => healthPoints; }

        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            maxHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);

            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(this.gameObject.name + " took damage: " + damage);

            healthPoints = Mathf.Max(0, healthPoints - damage);

            if (healthPoints == 0)
            {
                AwardExperience(instigator);
                Die();
            }
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints / maxHealthPoints);
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            if (isDead) return;

            var experiance = instigator.GetComponent<Experience>();
            if (experiance == null) return;

            experiance.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            if (regenationPercentage > 0)
            {
                var regenHealthPoint = GetComponent<BaseStats>().GetStat(Stat.Health) * regenationPercentage / 100;
                healthPoints = Mathf.Max(healthPoints, regenHealthPoint);
            }
            else
            {
                var newMaxHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
                if (newMaxHealthPoints > maxHealthPoints)
                {
                    var percentage = GetPercentage() / 100;

                    maxHealthPoints = newMaxHealthPoints;
                    healthPoints = maxHealthPoints * percentage;
                }
            }
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0)
            {
                Die();
            }
            else
            {
                GetComponent<Animator>().Play("Locomotion");
            }
        }
    }
}