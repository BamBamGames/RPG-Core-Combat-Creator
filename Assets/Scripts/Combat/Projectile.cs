using RPG.Core;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private bool isHoming = true;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 20;

        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 0f;

        [SerializeField] private bool canBeStucked = false;
        [SerializeField] private GameObject[] destroyedOnStuck = null;

        private Health target = null;
        private float damage = 0f;

        private Coroutine coroutine_Destroy;

        // Update is called once per frame
        private void Update()
        {
            if (target == null) return;

            if (isHoming && false == target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;

            transform.LookAt(GetAimLocation());

            coroutine_Destroy = StartCoroutine(_destroyedAfterLifeTime());
            //Destroy(this.gameObject, maxLifeTime);
        }

        private IEnumerator _destroyedAfterLifeTime()
        {
            yield return new WaitForSeconds(maxLifeTime);
            Destroy(this.gameObject);
        }

        private Vector3 GetAimLocation()
        {
            Vector3 aimPosition = target.transform.position;

            var targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule != null)
            {
                aimPosition += Vector3.up * targetCapsule.height / 2;
            }

            return aimPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), this.transform.rotation);
            }

            speed = 0;
            target.TakeDamage(damage);

            if (canBeStucked)
            {
                StuckInto(other);
            }
            else
            {
                DestroyOnCollider();
            }
        }

        private void StuckInto(Collider other)
        {
            StopCoroutine(coroutine_Destroy);

            // get stuck in dead target's collider
            this.transform.parent = other.transform;
            foreach (var toDestroy in destroyedOnStuck)
            {
                Destroy(toDestroy, lifeAfterImpact);
            }
        }

        private void DestroyOnCollider()
        {
            foreach (var toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(this.gameObject, lifeAfterImpact);
        }
    }
}