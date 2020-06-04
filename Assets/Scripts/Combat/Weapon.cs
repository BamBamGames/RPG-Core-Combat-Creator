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

        public float Damage { get => damage; }
        public float Range { get => range; }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (null != equippedPrefab)
            {
                var handTransform = isRightHanded ? rightHand : leftHand;
                Instantiate(equippedPrefab, handTransform);
            }

            if (null != animatornOverride)
            {
                animator.runtimeAnimatorController = animatornOverride;
            }
        }
    }
}