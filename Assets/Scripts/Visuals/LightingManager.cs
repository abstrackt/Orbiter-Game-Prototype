using UnityEngine;

namespace Visuals
{
    public class LightingManager : MonoBehaviour
    {
        public MapUtils map;
        public float cullDistance = 150f;
        
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            var extendedFrustum = _camera.orthographicSize * _camera.aspect * 1.5f;
            foreach (var planet in map.planets)
            {
                var dist = ((Vector2)planet.transform.position - (Vector2)_camera.transform.position).magnitude;
                if (planet.shadow.enabled && (dist > extendedFrustum || _camera.orthographicSize > cullDistance))
                {
                    planet.shadow.enabled = false;
                }
                else if (!planet.shadow.enabled && (dist < extendedFrustum && _camera.orthographicSize <= cullDistance))
                {
                    planet.shadow.enabled = true;
                }
            }
        }
    }
}