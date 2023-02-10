using System;
using System.Collections.Generic;
using Data.Map;
using Systems.Global;
using Systems.Physics;
using UnityEngine;
using Visuals.StarsScene;
using Random = UnityEngine.Random;

namespace Systems.StarsScene
{
    public class StarsMapManager : SingletonMonoBehaviour<StarsMapManager>
    {
        public GameObject starPrefab;
        public GameObject planetPrefab;
        public Transform parent;
        
        private StarsSpaceshipController _controller;
        private PhysicsSystem _physics;
        private SaveLoadManager _saves;

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
        
        public (PhysicsBody planet, float dist) ClosestPlanetPhysics => (_closestPlanet.physics, _planetDistance);
        
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
            _saves = SaveLoadManager.Instance;

            // Load from file later on
            var data = _saves.GetCurrentSave();
            LoadScene(data);
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

        public void LoadScene(WorldData data)
        {
            for (int i = 0; i < data.systems.Count; i++)
            {
                AddSystem(data.systems[i]);
            }
        }

        private void AddSystem(StarSystemData data)
        {
            AddStar(data.star);

            for (int i = 0; i < data.planets.Count; i++)
            {
                AddPlanet(data.planets[i], data.star);
            }
        }

        private void AddPlanet(PlanetData planet, StarData star)
        {
            var planetGo = Instantiate(planetPrefab, parent, true);
            var planetTr = planetGo.transform;
            var planetVis = planetGo.GetComponent<PlanetVisuals>();
            var planetPhysics = planetGo.GetComponent<PhysicsBody>();

            var orbitHeight = (planet.initPosition - star.initPosition).magnitude;
            var point = planet.initPosition;
            var accel = _physics.Gravity(point);
            var v = new Vector2(accel.normalized.y, -accel.normalized.x);
            var mag = (float)Math.Sqrt(PhysicsSystem.G * star.mass * PhysicsSystem.PhysicsScale / orbitHeight);
            v *= mag * (0.95f + Random.value * 0.1f);

            var planetSize = planet.radius;
            
            planetTr.position = point;
            planetPhysics.mass = planet.mass;
            planetPhysics.initialVelocity = v;
            
            var planetColor = planet.surfaceColor;

            planetTr.localScale = new Vector3(planetSize, planetSize, 1);
            planetVis.sprite.color = planetColor;
            var colorKeys = new GradientColorKey[]
            {
                new (planetColor, 0),
                new (planetColor, 1)
            };
            var alphaKeys = new GradientAlphaKey[]
            {
                new (15, 0),
                new (0, 1)
            };
            planetVis.trail.colorGradient.SetKeys(colorKeys, alphaKeys);
            
            _planets.Add(new PlanetEntry
            {
                data = planet,
                physics = planetPhysics,
                visuals = planetVis
            });
            
            _physics.AddBody(planetPhysics);
        }

        private void AddStar(StarData star)
        {
            var starGo = Instantiate(starPrefab, parent, true);
            var starTr = starGo.transform;
            var starVis = starGo.GetComponent<StarVisuals>();
            var att = starGo.GetComponent<Attractor>();

            starTr.position = star.initPosition;
            att.mass = star.mass;

            var starTemp = star.temperature;
            var starColor = new Color(
                (180 - starTemp * 0.75f) / 255f, 
                (180 - starTemp) / 255f, 
                (80 + starTemp * 1.5f) / 255f);
            var starSize = star.radius * 2;

            starVis.sprite.color = starColor;
            starVis.starLight.color = starColor;
            starVis.transform.localScale = new Vector3(starSize, starSize, 1);
            starVis.starLight.intensity = starSize * 0.75f;
            starVis.starLight.pointLightOuterRadius *= (float)Math.Sqrt(starSize) / 2;

            _physics.attractors.Add(att);
            _stars.Add(new StarEntry
            {
                data = star,
                physics = att,
                visuals = starVis
            });
        }

        public void SaveWorldState()
        {
            var world = _saves.GetCurrentSave();

            for (int i = 0; i < world.systems.Count; i++)
            {
                var system = world.systems[i];

                for (int j = 0; j < system.planets.Count; j++)
                {
                    var planet = _planets.Find(x => { return x.data.Equals(system.planets[j]); });

                    var data = system.planets[j];
                    data.initPosition = planet.physics.transform.position;
                    system.planets[j] = data;
                }

                world.systems[i] = system;
            }
            
            _saves.OverwriteSave(world);
        }
    }
}

