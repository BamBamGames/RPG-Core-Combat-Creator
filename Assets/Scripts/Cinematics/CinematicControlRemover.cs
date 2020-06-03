using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            GetComponent<PlayableDirector>().played += DisbaleControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void DisbaleControl(PlayableDirector p)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector p)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}