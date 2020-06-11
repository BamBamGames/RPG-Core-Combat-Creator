using UnityEngine;

namespace RPG.UI.DamageText
{
    /**
     * 附加在伤害文本对象上，仅用于销毁伤害文本
     */
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;

        public void DestroyTarget()
        {
            Destroy(targetToDestroy);
        }
    }
}