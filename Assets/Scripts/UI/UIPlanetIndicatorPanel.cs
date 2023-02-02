using System;
using Systems;
using Systems.StarsScene;
using UnityEngine;
using UnityEngine.UI;
using Visuals;

namespace UI
{
    public class UIPlanetIndicatorPanel : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public Image inhabitedIcon;
        public Text planetName;
        public StarsMapManager map;
        public float visibilityThreshold;
        
        private float _fade;
        private PlanetVisuals _closest;
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
        }
        
        public void Update()
        {
            var closestVis = map.ClosestPlanetVisuals;
            var closestData = map.ClosestPlanetData;
            _fade = Math.Min(_fade + Time.deltaTime, 1f);
            
            if (closestVis.planet != _closest)
            {
                _fade = 0;
            }

            _closest = closestVis.planet;

            inhabitedIcon.enabled = closestData.planet.Inhabited;
            
            var planetPos = closestVis.planet.transform.position;
            var screenPos = _camera.WorldToScreenPoint(planetPos);
            planetName.text = closestData.planet.planetName;
            transform.position = screenPos;
            canvasGroup.alpha = Math.Min(_fade, (visibilityThreshold - closestVis.dist) / visibilityThreshold);
        }
    }
}