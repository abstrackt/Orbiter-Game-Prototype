using System.Collections.Generic;
using Data.Map;
using Systems.Global;
using Systems.Physics;
using UnityEngine;
using Visuals;
using Visuals.StarsScene;

namespace Systems.StarsScene
{
    public class StarsMapManager : SingletonMonoBehaviour<StarsMapManager>
    {
        public GameObject starPrefab;
        public GameObject planetPrefab;
        public Transform parent;
        
        private StarsSpaceshipController _controller;
        private PhysicsSystem _physics;

        private struct PlanetEntry
        {
            public PlanetData data;
            public PlanetVisuals visuals;
            public PhysicsBody physics;
        }
        
        private struct StarEntry
        {
            public StarData data;
            public StarVisuals visuals;
            public Attractor physics;
        }
        
        public (StarData star, float dist) ClosestStarData => (_closestStar.data, _starDistance);
        public (PlanetData planet, float dist) ClosestPlanetData => (_closestPlanet.data, _planetDistance);
        
        public (StarVisuals star, float dist) ClosestStarVisuals => (_closestStar.visuals, _starDistance);
        public (PlanetVisuals planet, float dist) ClosestPlanetVisuals => (_closestPlanet.visuals, _planetDistance);
        
        private List<PlanetEntry> _planets = new ();
        private List<StarEntry> _stars = new ();

        private StarEntry _closestStar;
        private PlanetEntry _closestPlanet;

        private float _starDistance;
        private float _planetDistance;

        public void Start()
        {
            _controller = StarsSpaceshipController.Instance;
            _physics = PhysicsSystem.Instance;
        }
        
        public void Update()
        {
            _planetDistance = float.MaxValue;
            _closestPlanet = default;
            foreach (var planet in _planets)
            {
                var planetPos = planet.visuals.transform.position;
                var shipPos = _controller.transform.position;
                var distance = (planetPos - shipPos).magnitude;

                if (distance < _planetDistance)
                {
                    _closestPlanet = planet;
                    _planetDistance = distance;
                }
            }
            
            _starDistance = float.MaxValue;
            _closestStar = default;
            foreach (var star in _stars)
            {
                var starPos = star.visuals.transform.position;
                var shipPos = _controller.transform.position;
                var distance = (starPos - shipPos).magnitude;

                if (distance < _starDistance)
                {
                    _closestStar = star;
                    _starDistance = distance;
                }
            }
        }

        private void AddSystem(StarSystemData data)
        {
            // TODO
        }

        private void AddPlanet(PlanetData data, StarData star)
        {
            // TODO
        }

        private void AddStar(StarData star)
        {
            // TODO
        }
    }
}
