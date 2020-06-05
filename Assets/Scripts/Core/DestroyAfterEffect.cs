using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterEffect : MonoBehaviour
    {
        // Update is called once per frame
        private void Update()
        {
            if (false == GetComponent<ParticleSystem>().IsAlive())
            {
                Destroy(this.gameObject);
            }
        }
    }
}