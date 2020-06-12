using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;

        private void Update()
        {
            var percentage = healthComponent.GetPercentage();
            foreground.localScale = new Vector3(percentage, 1, 1);
        }
    }
}