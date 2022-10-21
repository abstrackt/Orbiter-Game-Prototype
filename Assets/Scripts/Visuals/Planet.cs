using Physics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Visuals
{
    [RequireComponent(typeof(PhysicsBody))]
    public class Planet : MonoBehaviour
    {
        public ShadowCaster2D shadow;
        public SpriteRenderer sprite;
        public TrailRenderer trail;
        public string name;
        public float showThreshold;
        public bool inhabited;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            if (_camera != null)
            {
                if (!trail.enabled)
                {
                    trail.Clear();
                }
                trail.enabled = _camera.orthographicSize < showThreshold;
            }
        }
    }
}