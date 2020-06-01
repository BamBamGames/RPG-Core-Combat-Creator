using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (false == other.gameObject.CompareTag("Player")) return;

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}