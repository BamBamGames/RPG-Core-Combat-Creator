using RPG.Control;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    // means automatic add Health component when add CombatTarget component
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        bool IRaycastable.HandleRaycast(PlayerController callingController)
        {
            if (false == callingController.GetComponent<Fighter>().CanAttack(this.gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(this.gameObject);
            }

            return true;
        }

        PlayerController.CursorType IRaycastable.GetCursorType()
        {
            return PlayerController.CursorType.Combat;
        }

    }
}