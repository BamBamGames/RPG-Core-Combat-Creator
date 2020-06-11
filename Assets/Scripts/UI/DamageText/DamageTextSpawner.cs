using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab = null;

        private void Start()
        {
            Spawn(11);
        }

        public void Spawn(float damage)
        {
            var damageTextInstance = Instantiate<DamageText>(damageTextPrefab, this.transform.position, this.transform.rotation);
        }
    }
}