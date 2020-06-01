using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            GetComponent<PlayableDirector>().played += DisbaleControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        void DisbaleControl(PlayableDirector p)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector p)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}