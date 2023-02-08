using System;
using Systems.StarsScene;
using UnityEngine;
using UnityEngine.UI;
using Visuals;
using Visuals.StarsScene;

namespace UI
{
    public class UIPlanetIndicatorPanel : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public Image inhabitedIcon;
        public Text planetName;
        public float visibilityThreshold;
        
        private float _fade;
        private PlanetVisuals _closest;
        private Camera _camera;
        private StarsMapManager _map;

        public void Start()
        {
            _camera = Camera.main;
            _map = StarsMapManager.Instance;
        }
        
        public void Update()
        {
            var closestVis = _map.ClosestPlanetVisuals;
            var closestData = _map.ClosestPlanetData;
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