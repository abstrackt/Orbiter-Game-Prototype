using System;
using UnityEngine;
using UnityEngine.UI;
using Visuals;

namespace UI
{
    public class UIPlanetIndicatorPanel : MonoBehaviour
    {
        public Camera camera;
        public CanvasGroup canvasGroup;
        public Image inhabitedIcon;
        public Text planetName;
        public MapUtils map;
        public float visibilityThreshold;
        
        private float _fade;
        private PlanetVisuals _closest;

        public void Update()
        {
            var closest = map.ClosestPlanet;
            _fade = Math.Min(_fade + Time.deltaTime, 1f);
            
            if (closest.planet != null)
            {
                if (closest.planet != _closest)
                {
                    _fade = 0;
                }

                _closest = closest.planet;

                inhabitedIcon.enabled = closest.planet.inhabited;
                
                var planetPos = closest.planet.transform.position;
                var screenPos = camera.WorldToScreenPoint(planetPos);
                planetName.text = closest.planet.planetName;
                transform.position = screenPos;
                canvasGroup.alpha = Math.Min(_fade, (visibilityThreshold - closest.dist) / visibilityThreshold);
            }
            else
            {
                canvasGroup.alpha = 0;
            }
        }
    }
}