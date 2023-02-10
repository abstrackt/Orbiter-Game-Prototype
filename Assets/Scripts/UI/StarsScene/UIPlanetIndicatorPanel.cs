using System;
using Systems.Global;
using Systems.StarsScene;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Visuals;
using Visuals.StarsScene;

namespace UI
{
    public class UIPlanetIndicatorPanel : UIPanel
    {
        public CanvasGroup canvasGroup;
        public Image inhabitedIcon;
        public Text planetName;
        public float visibilityThreshold;
        
        private float _fade;
        private PlanetVisuals _closest;
        private Camera _camera;
        private StarsMapManager _map;

        public override void Initialize(GameEventSystem events)
        {
            
        }

        public override void Deinitialize(GameEventSystem events)
        {
            
        }
        
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