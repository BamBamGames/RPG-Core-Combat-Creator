using Animancer;
using RPG.Core;
using RPG.Attributes;
using GameDevTV.Saving;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float maxNavPathLength = 40f;

        private NavMeshAgent navMeshAgent;
        private Health health;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            navMeshAgent.enabled = (false == health.IsDead());

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            // calculate the path
            NavMeshPath path = new NavMeshPath();
            var hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (false == hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            var velocity = navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);

            GetComponent<AnimationManager>().Move(localVelocity.z);
        }

        private float GetPathLength(NavMeshPath path)
        {
            // calculate path length by corner.
            float total = 0;
            for (int i = 1; i < path.corners.Length; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i - 1]);
            }

            return total;
        }

        [System.Serializable]
        private struct SaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 eulerAngles;
        }

        public object CaptureState()
        {
            /**
             * @ another implement by Dictionary
             * var data = new Dictionary<string, object>();
             * data["position"] = new SerializableVector3(transform.position);
             * data["rotation"] = new SerializableVector3(transform.eulerAngles);
             */
            var data = new SaveData();
            data.position = new SerializableVector3(transform.position);
            data.eulerAngles = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            /**
             * @ another implement by Dictionary
             * var data = (Dictionary<string, object>)state;
             * GetComponent<NavMeshAgent>().enabled = false;
             * transform.position = ((SerializableVector3)data["position"]).ToVector();
             * transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
             * GetComponent<NavMeshAgent>().enabled = true;
             */
            var data = (SaveData)state;
            navMeshAgent.enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.eulerAngles.ToVector();
            navMeshAgent.enabled = true;
        }
    }
}