using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject targetToDestroye = null;

        // Update is called once per frame
        private void Update()
        {
            if (false == GetComponent<ParticleSystem>().IsAlive())
            {
                if (targetToDestroye != null)
                {
                    Destroy(targetToDestroye);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}