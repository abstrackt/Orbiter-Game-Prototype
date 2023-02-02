using Systems.Physics;
using UnityEngine;

namespace Utils
{
    public class FocusController : MonoBehaviour
    {
        public PhysicsSystem physicsSystem;
        public Vector3 Followed => _followed == null ? Vector3.zero : _followed.position;
        public Vector3 Anchored => _anchored == null ? Vector3.zero : _anchored.position;

        [SerializeField] private Transform _followed;
        private Transform _anchored;

        public void Update()
        {
            var attractors = physicsSystem.attractors;

            if (attractors.Count <= 0)
                _anchored = null;

            var point = _followed.position;
            
            Attractor closest = attractors[0];
            var pos = closest.transform.position;
            var minDist = (point - pos).magnitude;
            foreach (var attractor in attractors)
            {
                pos = attractor.transform.position;
                var dist = (point - pos).magnitude;
                if (dist >= minDist) continue;
                closest = attractor;
                minDist = dist;
            }

            _anchored = closest.transform;
        }
    }
}