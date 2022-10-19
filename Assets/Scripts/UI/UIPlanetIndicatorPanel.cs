﻿using System;
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
        public MapDefinition map;
        public float visibilityThreshold;
        
        private float _fade;
        private Planet _closest;

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
                planetName.text = closest.planet.name;
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