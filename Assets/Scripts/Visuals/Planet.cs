using Physics;
using UnityEngine;

namespace Visuals
{
    [RequireComponent(typeof(PhysicsBody))]
    public class Planet : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public TrailRenderer trail;
        public float showThreshold;

        public void Update()
        {
            var mainCamera = Camera.main;
            if (mainCamera != null)
            {
                if (!trail.enabled)
                {
                    trail.Clear();
                }
                trail.enabled = mainCamera.orthographicSize < showThreshold;
            }
        }
    }
}