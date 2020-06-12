using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        private void Update()
        {
            var healthPercentage = healthComponent.GetPercentage();
            if (Mathf.Approximately(healthPercentage, 0) || Mathf.Approximately(healthPercentage, 1))
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthPercentage, 1, 1);
        }
    }
}