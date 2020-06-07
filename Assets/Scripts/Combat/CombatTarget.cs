using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    // means automatic add Health component when add CombatTarget component
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
    }
}