using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 7f;

    private Health target = null;

    // Update is called once per frame
    private void Update()
    {
        if (target == null) return;

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health target)
    {
        this.target = target;
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
}