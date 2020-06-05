using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatornOverride = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private float damage = 5f;
        [SerializeField] private float range = 2f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile = null;

        public float Damage { get => damage; }
        public float Range { get => range; }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (null != equippedPrefab)
            {
                Instantiate(equippedPrefab, GetTransform(rightHand, leftHand));
            }

            if (null != animatornOverride)
            {
                animator.runtimeAnimatorController = animatornOverride;
            }
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            var projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, damage);
        }
    }
}