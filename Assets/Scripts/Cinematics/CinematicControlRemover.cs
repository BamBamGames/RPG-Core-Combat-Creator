using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisbaleControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        void DisbaleControl(PlayableDirector p)
        {
            print("DisableControl");
        }

        void EnableControl(PlayableDirector p)
        {
            print("EnableControl");
        }
    }
}