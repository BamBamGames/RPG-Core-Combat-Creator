using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void MoveToCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            bool HasHit = Physics.Raycast(ray, out hit);
            if (HasHit)
            {
                GetComponent<Movement.Mover>().MoveTo(hit.point);
            }
        }
    }
}