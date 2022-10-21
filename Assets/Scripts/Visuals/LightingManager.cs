using UnityEngine;

namespace Visuals
{
    public class LightingManager : MonoBehaviour
    {
        public MapDefinition map;
        public float cullDistance = 150f;
        
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            foreach (var planet in map.planets)
            {
                if (planet.shadow.enabled && _camera.orthographicSize > cullDistance)
                {
                    planet.shadow.enabled = false;
                }
                else if (!planet.shadow.enabled && _camera.orthographicSize <= cullDistance)
                {
                    planet.shadow.enabled = true;
                }
            }
        }
    }
}