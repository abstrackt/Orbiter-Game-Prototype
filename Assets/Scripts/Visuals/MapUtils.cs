using System.Collections.Generic;
using Systems.Spaceship;
using UnityEngine;

namespace Visuals
{
    public class MapUtils : MonoBehaviour
    {
        public (StarVisuals star, float dist) ClosestStar => (_closestStar, _starDistance);
        public (PlanetVisuals planet, float dist) ClosestPlanet => (_closestPlanet, _planetDistance);

        public StarSceneSpaceshipController controller;
        public List<PlanetVisuals> planets = new ();
        public List<StarVisuals> stars = new ();

        private StarVisuals _closestStar;
        private PlanetVisuals _closestPlanet;

        private float _starDistance;
        private float _planetDistance;
        
        public void Update()
        {
            _planetDistance = float.MaxValue;
            _closestPlanet = null;
            foreach (var planet in planets)
            {
                var planetPos = planet.transform.position;
                var shipPos = controller.transform.position;
                var distance = (planetPos - shipPos).magnitude;

                if (distance < _planetDistance )
                {
                    _closestPlanet = planet;
                    _planetDistance = distance;
                }
            }
            
            _starDistance = float.MaxValue;
            _closestStar = null;
            foreach (var star in stars)
            {
                var starPos = star.transform.position;
                var shipPos = controller.transform.position;
                var distance = (starPos - shipPos).magnitude;

                if (distance < _starDistance)
                {
                    _closestStar = star;
                    _starDistance = distance;
                }
            }
        }
    }
}