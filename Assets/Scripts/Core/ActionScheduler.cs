using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        MonoBehaviour currentAction;

        public void StartAction(MonoBehaviour action)
        {
            if (action == currentAction) return;

            if (currentAction != null)
            {
                print("cancelling" + currentAction);
            }
            if (action != null)
            {
                print("start" + action);
            }

            currentAction = action;
        }
    }
}