using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool alreadyTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") == false) return;
            if (alreadyTriggered) return;

            GetComponent<PlayableDirector>().Play();
            alreadyTriggered = true;
        }
    }
}