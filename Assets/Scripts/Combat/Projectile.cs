using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private bool isHoming = true;
    [SerializeField] private bool canBeStucked = false;

    private Health target = null;
    private float damage = 0f;
    private bool stucked = false;

    // Update is called once per frame
    private void Update()
    {
        if (target == null) return;

        if (isHoming && false == target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }

        if (false == stucked)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;

        transform.LookAt(GetAimLocation());
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

        target.TakeDamage(damage);

        if (canBeStucked)
        {
            StuckInto(other);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void StuckInto(Collider other)
    {
        // get stuck in dead target's collider
        this.transform.parent = other.transform;
        stucked = true;
        transform.Find("Trail").gameObject.SetActive(false);
    }
}