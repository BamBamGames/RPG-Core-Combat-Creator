using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private Transform handTransform = null;
        [SerializeField] private AnimatorOverrideController weaponOverride = null;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            Spawnweapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void Spawnweapon()
        {
            Instantiate(weaponPrefab, handTransform);
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = weaponOverride;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
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
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        private void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }

            var targetToTest = combatTarget.GetComponent<Health>();
            return null != targetToTest && false == targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}