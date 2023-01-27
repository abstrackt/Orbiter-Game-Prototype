using System;
using Physics;
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
        public string planetName;
        public float showThreshold;
        public bool inhabited;
        
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
                trail.enabled = _camera.orthographicSize < showThreshold;
            }
        }
    }
}