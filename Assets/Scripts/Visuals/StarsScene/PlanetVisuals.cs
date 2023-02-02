using System;
using Systems.Physics;
using UnityEngine;

namespace Visuals
{
    [RequireComponent(typeof(PhysicsBody))]
    public class PlanetVisuals : MonoBehaviour
    {
        [NonSerialized] public Renderer planetRenderer;
        public UnityEngine.Rendering.Universal.ShadowCaster2D shadow;
        public SpriteRenderer sprite;
        public TrailRenderer trail;
        public float trailCull = 100f;
        public float shadowCull = 150f;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            planetRenderer = GetComponent<Renderer>();
        }

        public void Update()
        {
            if (_camera != null)
            {
                if (!trail.enabled)
                {
                    trail.Clear();
                }
                trail.enabled = _camera.orthographicSize < trailCull;
                
                var extendedFrustum = _camera.orthographicSize * _camera.aspect * 1.5f;
                var dist = ((Vector2)transform.position - (Vector2)_camera.transform.position).magnitude;
                if (shadow.enabled && (dist > extendedFrustum || _camera.orthographicSize > shadowCull))
                {
                    shadow.enabled = false;
                }
                else if (!shadow.enabled && (dist < extendedFrustum && _camera.orthographicSize <= shadowCull))
                {
                    shadow.enabled = true;
                }
            }
        }
    }
}