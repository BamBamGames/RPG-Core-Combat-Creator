using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Saving;
using RPG.Stats;
using UnityEngine;
using GameDevTV.Inventories;
using System;
using RPG.PlayableAnimation;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponConfig defaultWeaponConfig = null;

        private bool haveFakeTarget;
        private Health target;
        private Mover mover;
        private Equipment equipment;
        private float timeSinceLastAttack = Mathf.Infinity;
        private WeaponConfig currentWeaponConfig;
        private LazyValue<Weapon> currentWeapon;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            currentWeaponConfig = defaultWeaponConfig;
            currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private Weapon SetDefaultWeapon()
        {
            return AttachWeapon(defaultWeaponConfig);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (haveFakeTarget)
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

            // if (target == null) return;
            // if (target.IsDead()) return;
            //
            // if (!GetIsInRange(target.transform))
            // {
            //     GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            // }
            // else
            // {
            //     GetComponent<Mover>().Cancel();
            //     AttackBehaviour();
            // }
        }

        private void UpdateWeapon()
        {
            WeaponConfig weaponConfig = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weaponConfig == null)
            {
                EquipWeapon(defaultWeaponConfig);
            }
            else
            {
                EquipWeapon(weaponConfig);
            }
        }

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            currentWeapon.value = AttachWeapon(weaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            Animator animator = GetComponent<Animator>();
            return weaponConfig.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        // This will trigger the Hit() event.
        private void TriggerAttack()
        {
            GetComponent<PlayableAnimator>().PlayAttack();
            // GetComponent<Animator>().ResetTrigger("stopAttack");
            // GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        private void Hit()
        {
            if (target == null) return;

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            float calculatedDamage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, this.gameObject, calculatedDamage);
            }
            else
            {
                target.TakeDamage(this.gameObject, calculatedDamage);
            }
        }

        // Animation Event
        private void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.Range;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (false == mover.CanMoveTo(combatTarget.transform.position) &&
            false == GetIsInRange(combatTarget.transform))
            {
                return false;
            }

            var targetToTest = combatTarget.GetComponent<Health>();
            return null != targetToTest && false == targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            // target = combatTarget.GetComponent<Health>();
            haveFakeTarget = true;
        }

        public void Cancel()
        {
            target = null;
            haveFakeTarget = false;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            // GetComponent<Animator>().ResetTrigger("attack");
            // GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<PlayableAnimator>().PlayWalk(0);
        }

        object ISaveable.CaptureState()
        {
            // currentWeapon never be null, use Unarmed weapon instead null.
            return currentWeaponConfig.name;
        }

        void ISaveable.RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weaponConfig = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weaponConfig);
        }
    }
}