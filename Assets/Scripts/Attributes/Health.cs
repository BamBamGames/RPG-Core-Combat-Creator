using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [Tooltip("升级后血量的百分比, 设置为小于0关闭这个特性, 关闭特性后按照当前血量的百分比生成血量")]
        [SerializeField] private float regenationPercentage = -1f;
        [SerializeField] TakeDamageEvent takeDamage = null;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        private LazyValue<float> maxHealthPoints;
        private LazyValue<float> healthPoints;

        private bool isDead = false;

        public float MaxHealthPoints { get => maxHealthPoints.value; }
        public float HealthPoints { get => healthPoints.value; }

        private void Awake()
        {
            maxHealthPoints = new LazyValue<float>(GetInitialHealth);
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void Start()
        {
            maxHealthPoints.ForceInit();
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(0, healthPoints.value - damage);
            if (healthPoints.value == 0)
            {
                AwardExperience(instigator);
                Die();
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public float GetPercentage()
        {
            return healthPoints.value / maxHealthPoints.value;
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
                healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoint);
            }
            else
            {
                var newMaxHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
                if (newMaxHealthPoints > maxHealthPoints.value)
                {
                    var percentage = GetPercentage();

                    maxHealthPoints.value = newMaxHealthPoints;
                    healthPoints.value = maxHealthPoints.value * percentage;
                }
            }
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value == 0)
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